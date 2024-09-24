using System;

namespace Omedya.ChessLib.Core
{
    public class ChessSquare
    {
        public int X { get; }
        public int Y { get; }

        public ChessSquare(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        public ChessSquare Copy()
        {
            return new ChessSquare(this.X, this.Y);
        }
        
        #region Equality
        
        // Operators
        public static bool operator ==(ChessSquare left, ChessSquare right)
        {
            return left?.Equals(right) ?? right is null;
        }
        public static bool operator !=(ChessSquare left, ChessSquare right)
        {
            return !(left == right);
        }

        public static ChessSquare operator +(ChessSquare left, (int x, int y) direction)
        {
            return new ChessSquare(left.X + direction.x, left.Y + direction.y);
        }
        
        
        // Methods
        public override bool Equals(object obj)
        {
            return obj is ChessSquare other && Equals(other);
        }

        public bool Equals(ChessSquare other)
        {
            if (other is null) return false;
            
            return X == other.X && Y == other.Y;
        }
        
        public override int GetHashCode()
        {
            // Generate hash code by x and y
            return HashCode.Combine(X, Y);
        }
        
        #endregion
    }
}