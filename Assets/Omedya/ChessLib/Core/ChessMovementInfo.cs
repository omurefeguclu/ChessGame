namespace Omedya.ChessLib.Core
{
    public class ChessMovementInfo
    {
        public ChessSquare Start { get; }
        public ChessSquare End { get; }
        
        
        public ChessPiece StartPiece { get; }
        public ChessPiece EndPiece { get; }
        
        public ChessMovementInfo(ChessMovement movement, ChessBoardSnapshot boardSnapshot)
        {
            Start = movement.Start;
            End = movement.End;
            
            StartPiece = boardSnapshot.GetPiece(Start);
            EndPiece = boardSnapshot.GetPiece(End);
        }
    }
}