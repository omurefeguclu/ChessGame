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
        
        public abstract IEnumerable<ChessSquare> GetControlledSquares(ChessSquare position, ChessBoardSnapshot boardSnapshot);
        public abstract IEnumerable<ChessMovement> GetPossibleMovements(ChessSquare position, ChessBoardSnapshot boardSnapshot);

        public override string ToString()
        {
            return $"{Team} {GetType().Name}";
        }
    }
}