using Omedya.ChessLib.Core;

namespace Omedya.ChessLib.SpecialMovements
{
    public class EnPassantMove : ChessSpecialMovement
    {
        public ChessSquare CapturedPawnSquare { get; set; }
        
        public EnPassantMove(ChessSquare start, ChessSquare end, ChessSquare capturedPawnSquare) : base(start, end)
        {
            CapturedPawnSquare = capturedPawnSquare;
        }
        
        public override void Execute(ChessBoardSnapshot boardSnapshot)
        {
            boardSnapshot.SetPiece(CapturedPawnSquare, null);
        }
    }
}