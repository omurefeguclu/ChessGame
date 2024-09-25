using Omedya.ChessLib.Core;

namespace Omedya.ChessLib.SpecialMovements
{
    public class EnPassantMove : ChessSpecialMovement
    {
        public ChessSquare CapturedPawnSquare { get; set; }
        
        private ChessPiece _capturedPiece;
        private ChessPiece _currentPawnPiece;
        
        public EnPassantMove(ChessSquare start, ChessSquare end, ChessSquare capturedPawnSquare) : base(start, end)
        {
            CapturedPawnSquare = capturedPawnSquare;
        }
        
        public override void Execute(ChessBoardSnapshot boardSnapshot)
        {
            _currentPawnPiece = boardSnapshot.GetPiece(Start);
            _capturedPiece = boardSnapshot.GetPiece(CapturedPawnSquare);
            
            
            boardSnapshot.SetPiece(CapturedPawnSquare, null);
            boardSnapshot.SetPiece(Start, null);
            boardSnapshot.SetPiece(End, _currentPawnPiece);
        }

        public override void Rollback(ChessBoardSnapshot boardSnapshot)
        {
            boardSnapshot.SetPiece(CapturedPawnSquare, _capturedPiece);
            boardSnapshot.SetPiece(Start, _currentPawnPiece);
            boardSnapshot.SetPiece(End, null);
        }
    }
}