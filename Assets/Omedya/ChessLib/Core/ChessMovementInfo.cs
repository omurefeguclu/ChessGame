namespace Omedya.ChessLib.Core
{
    public class ChessMovementInfo
    {
        public ChessSquare Start { get; }
        public ChessSquare End { get; }
        public ChessPiece StartPiece { get; }
        public ChessPiece EndPiece { get; }
        
        
        public ChessMovementInfo(ChessMovement movement, ChessBoard board)
        {
            Start = movement.Start;
            End = movement.End;
            StartPiece = board.GetPiece(movement.Start);
            EndPiece = board.GetPiece(movement.End);
        }
    }
}