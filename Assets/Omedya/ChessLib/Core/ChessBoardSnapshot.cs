using System;
using System.Collections.Generic;
using System.Linq;
using Omedya.ChessLib.Pieces;
using Omedya.ChessLib.Util;
using UnityEngine;

namespace Omedya.ChessLib.Core
{
    public class ChessBoardSnapshot
    {
        public ChessTeam CurrentTurn { get; private set; }
        public ChessMovementInfo LastMovement { get; private set; }
        public Dictionary<(ChessTeam team, CastlingSide castlingSide), bool> CanCastle { get; }
        
        public List<ChessMovement> SavedPossibleMovements { get; private set; }
        public ChessBoard Board => _board;
        
        
        private readonly ChessPiece[,] _pieces;
        private readonly ChessBoard _board;

        
        #region Constructors

        public ChessBoardSnapshot(ChessBoard board)
        {
            _board = board;

            _pieces = new ChessPiece[board.Squares.GetLength(0), board.Squares.GetLength(1)];
            CanCastle = new()
            {
                { (ChessTeam.White, CastlingSide.KingSide), true },
                { (ChessTeam.White, CastlingSide.QueenSide), true },
                { (ChessTeam.Black, CastlingSide.KingSide), true },
                { (ChessTeam.Black, CastlingSide.QueenSide), true },
            };
        }

        public ChessBoardSnapshot(ChessBoard board, ChessPiece[,] pieces,
            Dictionary<(ChessTeam team, CastlingSide castlingSide), bool> canCastle)
        {
            _board = board;
            _pieces = pieces;
            
            // Copy canCastle dictionary
            CanCastle = new Dictionary<(ChessTeam team, CastlingSide castlingSide), bool>(canCastle);
        }
        
        #endregion
        
        public void Initialize()
        {
            CurrentTurn = ChessTeam.White;
            
            /* Setup default table with pieces */
            _pieces[0, 0] = new ChessRook(ChessTeam.White);
            _pieces[1, 0] = new ChessKnight(ChessTeam.White);
            _pieces[2, 0] = new ChessBishop(ChessTeam.White);
            _pieces[3, 0] = new ChessQueen(ChessTeam.White);
            _pieces[4, 0] = new ChessKing(ChessTeam.White);
            _pieces[5, 0] = new ChessBishop(ChessTeam.White);
            _pieces[6, 0] = new ChessKnight(ChessTeam.White);
            _pieces[7, 0] = new ChessRook(ChessTeam.White);
            for (var i = 0; i < 8; i++)
            {
                _pieces[i, 1] = new ChessPawn(ChessTeam.White);
            }
            
            _pieces[0, 7] = new ChessRook(ChessTeam.Black);
            _pieces[1, 7] = new ChessKnight(ChessTeam.Black);
            _pieces[2, 7] = new ChessBishop(ChessTeam.Black);
            _pieces[3, 7] = new ChessQueen(ChessTeam.Black);
            _pieces[4, 7] = new ChessKing(ChessTeam.Black);
            _pieces[5, 7] = new ChessBishop(ChessTeam.Black);
            _pieces[6, 7] = new ChessKnight(ChessTeam.Black);
            _pieces[7, 7] = new ChessRook(ChessTeam.Black);
            for (var i = 0; i < 8; i++)
            {
                _pieces[i, 6] = new ChessPawn(ChessTeam.Black);
            }
            
            SavedPossibleMovements = GetPossibleMovements().ToList();
        }
        

        internal IEnumerable<ChessMovement> GetPossibleMovements()
        {
            // Calculate possible movements for each piece
            for(int x = 1; x <= 8; x++)
            {
                for(int y = 1; y <= 8; y++)
                {
                    var square = _board.GetSquare(x, y);
                    var piece = GetPiece(square);
                    if(piece is null || piece.Team != CurrentTurn)
                        continue;

                    var possibleMovements = piece.GetPossibleMovements(square, this);

                    foreach (var movement in possibleMovements)
                    {
                        yield return movement;
                    }
                }
            }
        }

        private void PassTurn()
        {
            if (CurrentTurn == ChessTeam.None)
            {
                throw new System.Exception("Current turn is None");
            }
            else
            {
                CurrentTurn = CurrentTurn == ChessTeam.Black ? ChessTeam.White : ChessTeam.Black;    
            }
            
        }
        internal ChessSquare GetKingSquare(ChessTeam team)
        {
            for (var x = 1; x <= 8; x++)
            {
                for (var y = 1; y <= 8; y++)
                {
                    var square = _board.GetSquare(x, y);
                    var piece = GetPiece(square);

                    if (piece is ChessKing king && king.Team == team)
                        return square;
                }
            }

            throw new System.Exception("King not found");
        }
        
        
        public ChessPiece GetPiece(ChessSquare square)
        {
            return _pieces[square.X - 1, square.Y - 1];
        }
        public void SetPiece(ChessSquare square, ChessPiece piece)
        {
            _pieces[square.X - 1, square.Y - 1] = piece;
        }
        
        public void PerformMovement(ChessMovement movement)
        {
            var pieceToMove = GetPiece(movement.Start);
            if(pieceToMove is null)
                throw new System.Exception("There is no piece to move at the given position");
            
            // Saving last movement before snapshot changes
            LastMovement = new ChessMovementInfo(movement, this);
            
            SetPiece(movement.End, pieceToMove);
            SetPiece(movement.Start, null);
            
            PassTurn();
            SavedPossibleMovements = GetPossibleMovements().ToList();
            
            CheckCastleDisablement(pieceToMove, movement);
        }

        private void CheckCastleDisablement(ChessPiece pieceToMove, ChessMovement movement)
        {
            // Disable castling
            if (pieceToMove is ChessKing king)
            {
                if (king.Team == ChessTeam.White)
                {
                    CanCastle[(ChessTeam.White, CastlingSide.KingSide)] = false;
                    CanCastle[(ChessTeam.White, CastlingSide.QueenSide)] = false;
                }
                else
                {
                    CanCastle[(ChessTeam.Black, CastlingSide.KingSide)] = false;
                    CanCastle[(ChessTeam.Black, CastlingSide.QueenSide)] = false;
                }
            }
            else if (pieceToMove is ChessRook rook)
            {
                if (rook.Team == ChessTeam.White)
                {
                    if (movement.Start == (1, 1))
                    {
                        CanCastle[(ChessTeam.White, CastlingSide.QueenSide)] = false;
                    }
                    else if (movement.Start == (8, 1))
                    {
                        CanCastle[(ChessTeam.White, CastlingSide.KingSide)] = false;
                    }
                }
                else
                {
                    if (movement.Start == (1, 8))
                    {
                        CanCastle[(ChessTeam.Black, CastlingSide.QueenSide)] = false;
                    }
                    else if (movement.Start == (8, 8))
                    {
                        CanCastle[(ChessTeam.Black, CastlingSide.KingSide)] = false;
                    }
                }
            }
        }
        // Returns rollback action
        internal RollbackUtil PerformTemporaryMovement(ChessMovement movement)
        {
            var pieceToMove = GetPiece(movement.Start);
            if(pieceToMove is null)
                throw new System.Exception("There is no piece to move at the given position");
        
            
            SetPiece(movement.End, pieceToMove);
            SetPiece(movement.Start, null);
            PassTurn();

            
            void RollbackAction()
            {
                Debug.Log($"Rolling back the movement: {movement.Start} -> {movement.End}");
                var oldPiece = GetPiece(movement.End);
                
                SetPiece(movement.Start, pieceToMove);
                SetPiece(movement.End, oldPiece);

                PassTurn();
            }
            
            return new RollbackUtil(RollbackAction);
        }
        
        internal ChessBoardSnapshot Copy()
        {
            // Copy pieces
            var snapshot = new ChessBoardSnapshot(_board, _pieces, CanCastle);
            snapshot.CurrentTurn = CurrentTurn;
            
            return snapshot;
        }
    }
}