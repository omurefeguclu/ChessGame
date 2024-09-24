using Omedya.ChessLib.Core;
using UnityEngine;

namespace Omedya.ChessLib.SpecialMovements
{
    public class CastlingMove : ChessSpecialMovement
    {
        public ChessSquare RookSquare { get; set; }
        
        public CastlingMove(ChessSquare start, ChessSquare end, ChessSquare rookSquare) : base(start, end)
        {
            RookSquare = rookSquare;
        }


        public override void Execute(ChessBoardSnapshot boardSnapshot)
        {
            boardSnapshot.SetPiece(RookSquare, null);

            var rookOffset = (RookSquare - Start);
            var rookDirection = (rookOffset.x / Mathf.Abs(rookOffset.x), rookOffset.y / Mathf.Abs(rookOffset.y));
            
            boardSnapshot.SetPiece(End + rookDirection, boardSnapshot.GetPiece(RookSquare));
        }
    }
}