namespace Omedya.ChessLib.Core
{
    public abstract class ChessSpecialMovement : ChessMovement
    {
        protected ChessSpecialMovement(ChessSquare start, ChessSquare end) : base(start, end)
        {
        }
        
        public abstract void Execute(ChessBoardSnapshot boardSnapshot);
    }
}