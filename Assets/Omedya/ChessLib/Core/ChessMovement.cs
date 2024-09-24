namespace Omedya.ChessLib.Core
{
    public class ChessMovement
    {
        public ChessSquare Start { get; }
        public ChessSquare End { get; }
        
        public ChessMovement(ChessSquare start, ChessSquare end)
        {
            Start = start;
            End = end;
        }
        
    }
}