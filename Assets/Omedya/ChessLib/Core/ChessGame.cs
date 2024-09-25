using System.Collections.Generic;
using System.Linq;
using Omedya.ChessLib.Util;

namespace Omedya.ChessLib.Core
{
    public class ChessGame
    {
        public static ChessGame CreateDefaultGame() => new ChessGame();
        
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
            CurrentSnapshot.Initialize();
            
            Winner = ChessTeam.None;
            IsFinished = false;
        }
        
        // Constructor with info
        public ChessGame(ChessTeam winner, bool isFinished)
        {
            Winner = winner;
            IsFinished = isFinished;
        }

        public bool MovePiece(ChessMovement movement)
        {
            if (IsFinished)
                return false;

            // Check if the movement is valid
            movement = CurrentSnapshot.GetPossibleMovement(movement.Start, movement.End);
            
            var beforeSnapshot = CurrentSnapshot;
            
            var newSnapshot = CurrentSnapshot.NewSnapshotByCopy();
            newSnapshot.PerformMovement(movement);
            
            CurrentSnapshot = newSnapshot;
            
            SnapshotsHistory.Add(beforeSnapshot);
            
            
            if (!CurrentSnapshot.AnyMovesAvailable)
            {
                if (MovementValidationUtil.IsKingInCheck(CurrentSnapshot, CurrentSnapshot.CurrentTurn))
                {
                    Winner = CurrentSnapshot.CurrentTurn == ChessTeam.White ? ChessTeam.Black : ChessTeam.White;
                }
                else
                {
                    Winner = ChessTeam.None;
                }
                
                IsFinished = true;
            }

            return true;
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