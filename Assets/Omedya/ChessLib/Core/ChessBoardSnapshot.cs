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
            SetPiece(new ChessSquare(1, 1), new ChessRook(ChessTeam.White));
            SetPiece(new ChessSquare(2, 1), new ChessKnight(ChessTeam.White));
            SetPiece(new ChessSquare(3, 1), new ChessBishop(ChessTeam.White));
            SetPiece(new ChessSquare(4, 1), new ChessQueen(ChessTeam.White));
            SetPiece(new ChessSquare(5, 1), new ChessKing(ChessTeam.White));
            SetPiece(new ChessSquare(6, 1), new ChessBishop(ChessTeam.White));
            SetPiece(new ChessSquare(7, 1), new ChessKnight(ChessTeam.White));
            SetPiece(new ChessSquare(8, 1), new ChessRook(ChessTeam.White));
            
            SetPiece(new ChessSquare(1, 8), new ChessRook(ChessTeam.Black));
            SetPiece(new ChessSquare(2, 8), new ChessKnight(ChessTeam.Black));
            SetPiece(new ChessSquare(3, 8), new ChessBishop(ChessTeam.Black));
            SetPiece(new ChessSquare(4, 8), new ChessQueen(ChessTeam.Black));
            SetPiece(new ChessSquare(5, 8), new ChessKing(ChessTeam.Black));
            SetPiece(new ChessSquare(6, 8), new ChessBishop(ChessTeam.Black));
            SetPiece(new ChessSquare(7, 8), new ChessKnight(ChessTeam.Black));
            SetPiece(new ChessSquare(8, 8), new ChessRook(ChessTeam.Black));

            for (var i = 1; i <= 8; i++)
            {
                SetPiece(new ChessSquare(i, 2), new ChessPawn(ChessTeam.White));
                SetPiece(new ChessSquare(i, 7), new ChessPawn(ChessTeam.Black));
            }
            
            CalculatePossibleMovements();
        }


        internal IEnumerable<ChessSquare> GetAttackedSquares(ChessTeam friendlyTeam)
        {
            for (var x = 1; x <= 8; x++)
            {
                for (var y = 1; y <= 8; y++)
                {
                    var square = _board.GetSquare(x, y);
                    var piece = GetPiece(square);

                    if (piece is null || piece.Team == friendlyTeam)
                        continue;

                    foreach (var controlledSquare in piece.GetControlledSquares(square, this))
                    {
                        yield return controlledSquare;
                    }
                }
            }
        }
        private void CalculatePossibleMovements()
        {
            SavedPossibleMovements = new List<ChessMovement>();
            
            // Calculate possible movements for each piece
            for(int x = 1; x <= _board.Squares.GetLength(0); x++)
            {
                for(int y = 1; y <= _board.Squares.GetLength(1); y++)
                {
                    var square = _board.GetSquare(x, y);
                    if (square is null)
                        continue;
                    
                    var piece = GetPiece(square);
                    if(piece is null || piece.Team != CurrentTurn)
                        continue;

                    var possibleMovements = piece.GetPossibleMovements(square, this);

                    SavedPossibleMovements.AddRange(possibleMovements);
                    
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
            if (square is null)
                throw new System.ArgumentNullException(nameof(square));
            
            return _pieces[square.X - 1, square.Y - 1];;
        }
        public void SetPiece(ChessSquare square, ChessPiece piece)
        {
            if (square is null)
                throw new System.ArgumentNullException(nameof(square));
            
            _pieces[square.X - 1, square.Y - 1] = piece;
        }
        
        private void PerformMovementCore(ChessPiece pieceToMove, ChessMovement movement)
        {
            if (movement is ChessSpecialMovement specialMovement)
            {
                specialMovement.Execute(this);
            }
            else
            {
                SetPiece(movement.End, pieceToMove);
                SetPiece(movement.Start, null);
            }
        }
        
        public void PerformMovement(ChessMovement movement)
        {
            var pieceToMove = GetPiece(movement.Start);
            if(pieceToMove is null)
                throw new System.Exception("There is no piece to move at the given position");
            
            // Saving last movement before snapshot changes
            LastMovement = new ChessMovementInfo(movement, this);
            
            PerformMovementCore(pieceToMove, movement);
            
            PassTurn();
            CalculatePossibleMovements();
            
            CheckCastleDisablement(pieceToMove, movement);
        }
        // Returns rollback action
        internal RollbackUtil PerformTemporaryMovement(ChessMovement movement)
        {
            var pieceToMove = GetPiece(movement.Start);
            if(pieceToMove is null)
                throw new System.Exception("There is no piece to move at the given position");
        
            var oldPiece = GetPiece(movement.End);
            
            PerformMovementCore(pieceToMove, movement);
            PassTurn();

            return new RollbackUtil(RollbackAction);

            void RollbackAction()
            {
                if(movement is ChessSpecialMovement specialMovement)
                {
                    specialMovement.Rollback(this);
                }
                else
                {
                    SetPiece(movement.Start, pieceToMove);
                    SetPiece(movement.End, oldPiece);
                }

                PassTurn();
            }
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
        
        
        internal ChessBoardSnapshot Copy()
        {
            // Copy pieces
            var snapshot = new ChessBoardSnapshot(_board, _pieces, CanCastle);
            snapshot.CurrentTurn = CurrentTurn;
            
            return snapshot;
        }
    }
}