namespace Omedya.ChessLib.Constants
{
    public class MovementConstants
    {
        public static readonly (int, int)[] KnightMovements = {
            (-2, -1),
            (-2, 1),
            (-1, -2),
            (-1, 2),
            (1, -2),
            (1, 2),
            (2, -1),
            (2, 1)
        };
        
        public static readonly (int, int)[] CardinalDirections = {
            (0, 1),
            (0, -1),
            (1, 0),
            (-1, 0)
        };
        public static readonly (int, int)[] DiagonalDirections = {
            (1, 1),
            (1, -1),
            (-1, 1),
            (-1, -1)
        };
        public static readonly (int, int)[] AllDirections = {
            (0, 1),
            (0, -1),
            (1, 0),
            (-1, 0),
            (1, 1),
            (1, -1),
            (-1, 1),
            (-1, -1)
        };
        
    }
}