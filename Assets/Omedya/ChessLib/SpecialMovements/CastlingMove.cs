using Omedya.ChessLib.Core;
using Omedya.ChessLib.Extensions;
using UnityEngine;

namespace Omedya.ChessLib.SpecialMovements
{
    public class CastlingMove : ChessSpecialMovement
    {
        // Ignore these in serialization
        public CastlingSide CastlingSide { get; }
        public ChessSquare RookStartSquare { get; set; }
        public ChessSquare RookEndSquare { get; set; }
        
        
        private ChessPiece _rookPiece;
        private ChessPiece _kingPiece;
        
        public CastlingMove(ChessSquare start, ChessSquare end, CastlingSide castlingSide, ChessSquare rookStartSquare) : base(start, end)
        {
            CastlingSide = castlingSide;
            RookStartSquare = rookStartSquare;
            
            var rookOffset = (End - RookStartSquare);
            var rookDirection = rookOffset.Sign();
            
            RookEndSquare = End + rookDirection;
        }


        public override void Execute(ChessBoardSnapshot boardSnapshot)
        {
            // Log castling movement information
            Debug.Log($"Castling move: {Start} -> {End}, Rook: {RookStartSquare} -> {RookEndSquare}");
            
            _rookPiece = boardSnapshot.GetPiece(RookStartSquare);
            _kingPiece = boardSnapshot.GetPiece(Start);
            
            boardSnapshot.SetPiece(RookStartSquare, null);
            
            boardSnapshot.SetPiece(Start, null);
            boardSnapshot.SetPiece(RookStartSquare, null);
            
            boardSnapshot.SetPiece(RookEndSquare, _rookPiece);
            boardSnapshot.SetPiece(End, _kingPiece);
        }

        public override void Rollback(ChessBoardSnapshot boardSnapshot)
        {
            boardSnapshot.SetPiece(RookStartSquare, _rookPiece);
            boardSnapshot.SetPiece(Start, _kingPiece);
            
            boardSnapshot.SetPiece(RookEndSquare, null);
            boardSnapshot.SetPiece(End, null);
            
        }
    }
}