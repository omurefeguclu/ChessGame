using System.Collections.Generic;
using System.Linq;
using Omedya.ChessLib.Pieces;
using Omedya.ChessLib.Util;

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

        public ChessBoardSnapshot(ChessBoard board, ChessPiece[,] pieces, Dictionary<(ChessTeam team, CastlingSide castlingSide), bool> canCastle)
        {
            _board = board;
            _pieces = pieces;
            
            // Copy canCastle dictionary
            CanCastle = new Dictionary<(ChessTeam team, CastlingSide castlingSide), bool>(canCastle);
        }
        
        #endregion
        
        

        internal IEnumerable<ChessMovement> GetPossibleMovements()
        {
            // Calculate possible movements for each piece
            for(int x = 0; x < 8; x++)
            {
                for(int y = 0; y < 8; y++)
                {
                    var piece = _pieces[x, y];
                    if(piece is null || piece.Team != CurrentTurn)
                        continue;

                    var possibleMovements = piece.GetPossibleMovements(_board.GetSquare(x, y), this);

                    foreach (var movement in possibleMovements)
                    {
                        yield return movement;
                    }
                }
            }
        }

        public void PassTurn()
        {
            if (CurrentTurn == ChessTeam.None)
            {
                CurrentTurn = ChessTeam.White;   
            }
            else
            {
                CurrentTurn = CurrentTurn == ChessTeam.Black ? ChessTeam.White : ChessTeam.Black;    
            }
            
        }
        internal ChessSquare GetKingSquare(ChessTeam team)
        {
            for (var x = 0; x < 8; x++)
            {
                for (var y = 0; y < 8; y++)
                {
                    var piece = _pieces[x, y];

                    if (piece is ChessKing king && king.Team == team)
                        return _board.GetSquare(x, y);
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
            
            _pieces[movement.End.X, movement.End.Y] = pieceToMove;
            _pieces[movement.Start.X, movement.Start.Y] = null;
            
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
        
            var oldPiece = GetPiece(movement.End);
        
            _pieces[movement.End.X, movement.End.Y] = pieceToMove;
            _pieces[movement.Start.X, movement.Start.Y] = null;
            PassTurn();
            
            return new RollbackUtil(() =>
            {
                _pieces[movement.Start.X, movement.Start.Y] = pieceToMove;
                _pieces[movement.End.X, movement.End.Y] = oldPiece;
                
                PassTurn();
            });
        }
        
        internal ChessBoardSnapshot Copy()
        {
            // Copy pieces
            
            return new ChessBoardSnapshot(_board, _pieces, CanCastle);
        }
    }
}