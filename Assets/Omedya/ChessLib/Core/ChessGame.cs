namespace Omedya.ChessLib.Core
{
    public class ChessGame
    {
        public static ChessGame Default => new ChessGame();
        
        public ChessTeam CurrentTurn { get; private set; }
        public ChessTeam Winner { get; private set; }
        public bool IsFinished { get; private set; }
        
        private ChessGame()
        {
            CurrentTurn = ChessTeam.White;
            Winner = ChessTeam.None;
            IsFinished = false;
        }
        
        public ChessGame(ChessTeam currentTurn, ChessTeam winner, bool isFinished)
        {
            CurrentTurn = currentTurn;
            Winner = winner;
            IsFinished = isFinished;
        }
    }
}