namespace Omedya.ChessLib.Core
{
    public class ChessBoard
    {
        // Seperating squares from board to be able to implement custom game modes that have different board shapes
        public readonly ChessSquare[,] Squares;
    
        public int Width => Squares.GetLength(0);
        public int Height => Squares.GetLength(1);
        
        public static ChessBoard CreateDefaultBoard()
        {
            var squares = new ChessSquare[8, 8];
            for (var x = 0; x < 8; x++)
            {
                for (var y = 0; y < 8; y++)
                {
                    squares[x, y] = new ChessSquare(x + 1, y + 1);
                }
            }
        
            var board = new ChessBoard(squares);
        
            return board;
        }
    
        public ChessBoard(ChessSquare[,] squares)
        {
            Squares = squares;
        }

        #region Getters
    
        public ChessSquare GetSquare(int x, int y)
        {
            // Check for out of bounds
            if(x < 1 || x > Width || y < 1 || y > Height)
                return null;
            
            return Squares[x - 1, y - 1];
        }
    
        #endregion

    
    
    
    
    }    
}

