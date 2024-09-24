using System;
using System.Collections;
using System.Collections.Generic;
using Omedya.ChessLib.Core;
using Omedya.ChessLib.Pieces;
using Omedya.ChessLib.Util;
using UnityEngine;

public class ChessBoard
{
    private readonly ChessPiece[,] _pieces;
    // Seperating squares from board to be able to implement custom game modes that have different board shapes
    private readonly ChessSquare[,] _squares;
    
    public static ChessBoard CreateDefaultBoard()
    {
        var pieces = new ChessPiece[8, 8];
        var squares = new ChessSquare[8, 8];
        
        var board = new ChessBoard(pieces, squares);
        board.InitializeDefaultBoard();
        
        return board;
    }
    
    public ChessBoard(ChessPiece[,] pieces, ChessSquare[,] squares)
    {
        _pieces = pieces;
        _squares = squares;
    }

    #region Initialization
    
    private void InitializeDefaultBoard()
    {
        for (var x = 0; x < 8; x++)
        {
            for (var y = 0; y < 8; y++)
            {
                _squares[x, y] = new ChessSquare(x, y);
            }
        }
        
        // Initialize pieces
        
    }
    
    #endregion
    
    #region Getters
    
    public ChessSquare GetSquare(int x, int y)
    {
        return _squares[x, y];
    }
    public ChessPiece GetPiece(int x, int y)
    {
        return GetPiece(GetSquare(x, y));
    }
    public ChessPiece GetPiece(ChessSquare square)
    {
        return _pieces[square.X, square.Y];
    }

    public ChessSquare GetKingSquare(ChessTeam team)
    {
        for (var x = 0; x < 8; x++)
        {
            for (var y = 0; y < 8; y++)
            {
                var piece = _pieces[x, y];

                if (piece is ChessKing king && king.Team == team)
                    return GetSquare(x, y);
            }
        }

        throw new System.Exception("King not found");
    }
    
    #endregion

    public void PerformMovement(ChessMovement movement)
    {
        var pieceToMove = GetPiece(movement.Start);
        if(pieceToMove is null)
            throw new System.Exception("There is no piece to move at the given position");
        
        _pieces[movement.End.X, movement.End.Y] = pieceToMove;
        _pieces[movement.Start.X, movement.Start.Y] = null;
        
        // Calculate new possible movements
    }
    // Returns rollback action
    public RollbackUtil PerformTemporaryMovement(ChessMovement movement)
    {
        var pieceToMove = GetPiece(movement.Start);
        if(pieceToMove is null)
            throw new System.Exception("There is no piece to move at the given position");
        
        var oldPiece = GetPiece(movement.End);
        
        _pieces[movement.End.X, movement.End.Y] = pieceToMove;
        _pieces[movement.Start.X, movement.Start.Y] = null;

        return new RollbackUtil(() =>
        {
            _pieces[movement.Start.X, movement.Start.Y] = pieceToMove;
            _pieces[movement.End.X, movement.End.Y] = oldPiece;
        });
    }
    
    
    public ChessBoard Copy()
    {
        // Copy pieces
        var newPieces = new ChessPiece[8, 8];
        for (var x = 0; x < 8; x++)
        {
            for (var y = 0; y < 8; y++)
            {
                newPieces[x, y] = _pieces[x, y]?.Copy();
            }
        }
        
        // Copy squares
        var newSquares = new ChessSquare[8, 8];
        for (var x = 0; x < 8; x++)
        {
            for (var y = 0; y < 8; y++)
            {
                newSquares[x, y] = _squares[x, y].Copy();
            }
        }
        
        return new ChessBoard(newPieces, newSquares);
    }
}
