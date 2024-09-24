using System.Collections.Generic;

namespace Omedya.ChessLib.Core
{
    public abstract class ChessPiece
    {
        public ChessTeam Team { get; private set; }

        protected ChessPiece(ChessTeam team)
        {
            Team = team;
        }
        
        public ChessPiece Copy()
        {
            return (ChessPiece) MemberwiseClone();
        }

        public abstract IEnumerable<ChessMovement> GetPossibleMovements(ChessSquare position, ChessBoardSnapshot boardSnapshot);
    }
}