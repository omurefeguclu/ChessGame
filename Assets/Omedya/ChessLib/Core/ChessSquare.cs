using System;
using Omedya.ChessLib.Util;

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

        public override string ToString()
        {
            return NotationUtil.GetSquareNotation(this);
        }

        #region Equality
        
        // Operators
        public static bool operator ==(ChessSquare left, (int x, int y) right)
        {
            return left?.Equals(right) ?? false;
        }

        public static bool operator !=(ChessSquare left, (int x, int y) right)
        {
            return !(left == right);
        }

        public static bool operator ==(ChessSquare left, ChessSquare right)
        {
            return left?.Equals(right) ?? right is null;
        }
        public static bool operator !=(ChessSquare left, ChessSquare right)
        {
            return !(left == right);
        }

        // Addition
        public static ChessSquare operator +(ChessSquare left, (int x, int y) direction)
        {
            return new ChessSquare(left.X + direction.x, left.Y + direction.y);
        }
        public static ChessSquare operator +(ChessSquare left, ChessSquare right)
        {
            return new ChessSquare(left.X + right.X, left.Y + right.Y);
        }
        // Subtraction
        public static ChessSquare operator -(ChessSquare left, (int x, int y) direction)
        {
            return new ChessSquare(left.X - direction.x, left.Y - direction.y);
        }
        public static (int x, int y) operator -(ChessSquare left, ChessSquare right)
        {
            return (left.X - right.X, left.Y - right.Y);
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
        public bool Equals((int x, int y) other)
        {
            return X == other.x && Y == other.y;
        }
        
        
        public override int GetHashCode()
        {
            // Generate hash code by x and y
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }
        
        #endregion
    }
}