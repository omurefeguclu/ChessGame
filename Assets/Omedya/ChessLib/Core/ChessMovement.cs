using Omedya.ChessLib.Util;

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

        // Equality check
        public override bool Equals(object obj)
        {
            if (obj is ChessMovement other)
            {
                return Equals(other);
            }

            return false;
        }
        public bool Equals(ChessMovement other)
        {
            return Start == other.Start && End == other.End;
        }
        
        public static bool operator ==(ChessMovement a, ChessMovement b)
        {
            return a?.Equals(b) ?? b is null;
        }
        public static bool operator !=(ChessMovement a, ChessMovement b)
        {
            return !(a == b);
        }
        
        public override int GetHashCode()
        {
            unchecked
            {
                return (Start.GetHashCode() * 397) ^ End.GetHashCode();
            }
        }
    }
}