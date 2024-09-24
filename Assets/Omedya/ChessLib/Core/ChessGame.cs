using System.Collections.Generic;
using System.Linq;

namespace Omedya.ChessLib.Core
{
    public class ChessGame
    {
        public static ChessGame Default => new ChessGame();
        
        public ChessTeam Winner { get; private set; }
        public bool IsFinished { get; private set; }
        
        public ChessBoard Board { get; private set; }
        public ChessBoardSnapshot CurrentSnapshot { get; private set; }
        
        // Board snapshot history
        public List<ChessBoardSnapshot> SnapshotsHistory { get; } = new List<ChessBoardSnapshot>();
        
        // Constructor for new game
        private ChessGame()
        {
            Board = ChessBoard.CreateDefaultBoard();
            CurrentSnapshot = new ChessBoardSnapshot(Board);
            CurrentSnapshot.PassTurn();
            
            Winner = ChessTeam.None;
            IsFinished = false;
        }
        
        // Constructor with info
        public ChessGame(ChessTeam winner, bool isFinished)
        {
            Winner = winner;
            IsFinished = isFinished;
        }

        public void MovePiece(ChessMovement movement)
        {
            if (IsFinished)
                return;
            
            if (!CurrentSnapshot.GetPossibleMovements().Contains(movement))
                return;
            
            var beforeSnapshot = CurrentSnapshot;
            
            var newSnapshot = CurrentSnapshot.Copy();
            newSnapshot.PerformMovement(movement);
            
            CurrentSnapshot = newSnapshot;
            
            SnapshotsHistory.Add(beforeSnapshot);
            
            
            if (CurrentSnapshot.SavedPossibleMovements.Count == 0)
            {
                Winner = CurrentSnapshot.CurrentTurn == ChessTeam.White ? ChessTeam.Black : ChessTeam.White;
                IsFinished = true;
            }
        }

        public void RollbackToSnapshot(int index)
        {
            if (index < 0 || index >= SnapshotsHistory.Count)
                return;
            
            CurrentSnapshot = SnapshotsHistory[index];
            SnapshotsHistory.RemoveRange(index, SnapshotsHistory.Count - index);
        }
    }
}