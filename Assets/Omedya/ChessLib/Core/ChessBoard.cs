using System;
using System.Collections;
using System.Collections.Generic;
using Omedya.ChessLib.Core;
using Omedya.ChessLib.Pieces;
using Omedya.ChessLib.Util;
using UnityEngine;

public class ChessBoard
{
    // Seperating squares from board to be able to implement custom game modes that have different board shapes
    public readonly ChessSquare[,] Squares;
    
    public static ChessBoard CreateDefaultBoard()
    {
        var squares = new ChessSquare[8, 8];
        
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
        return Squares[x - 1, y - 1];
    }
    
    #endregion

    
    
    
    
}
