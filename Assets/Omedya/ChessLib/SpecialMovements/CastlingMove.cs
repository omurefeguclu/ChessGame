using Omedya.ChessLib.Core;
using UnityEngine;

namespace Omedya.ChessLib.SpecialMovements
{
    public class CastlingMove : ChessSpecialMovement
    {
        public CastlingSide CastlingSide { get; }
        public ChessSquare RookSquare { get; set; }
        
        private ChessSquare _newRookSquare;
        private ChessPiece _rookPiece;
        private ChessPiece _kingPiece;
        
        public CastlingMove(ChessSquare start, ChessSquare end, CastlingSide castlingSide, ChessSquare rookSquare) : base(start, end)
        {
            CastlingSide = castlingSide;
            RookSquare = rookSquare;
        }


        public override void Execute(ChessBoardSnapshot boardSnapshot)
        {
            _rookPiece = boardSnapshot.GetPiece(RookSquare);
            _kingPiece = boardSnapshot.GetPiece(Start);
            
            boardSnapshot.SetPiece(RookSquare, null);
            
            var rookOffset = (RookSquare - Start);
            var rookDirection = ((int)Mathf.Sign(rookOffset.x), (int)Mathf.Sign(rookOffset.y));
            
            _newRookSquare = End + rookDirection;
            
            boardSnapshot.SetPiece(Start, null);
            boardSnapshot.SetPiece(RookSquare, null);
            
            boardSnapshot.SetPiece(_newRookSquare, _rookPiece);
            boardSnapshot.SetPiece(End, _kingPiece);
        }

        public override void Rollback(ChessBoardSnapshot boardSnapshot)
        {
            boardSnapshot.SetPiece(RookSquare, _rookPiece);
            boardSnapshot.SetPiece(Start, _kingPiece);
            
            boardSnapshot.SetPiece(_newRookSquare, null);
            boardSnapshot.SetPiece(End, null);
            
        }
    }
}