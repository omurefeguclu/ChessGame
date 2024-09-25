using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Omedya.ChessLib.Core;
using Omedya.ChessLib.Pieces;

public class ChessGameInitializationTests
{
    private ChessGame _chessGame;
    
    [SetUp]
    public void Init()
    {
        _chessGame = ChessGame.CreateDefaultGame();
    }
    
    [Test]
    public void ChessGame_DefaultGame_ShouldBeInitialized()
    {
        Debug.Log("ChessGame_DefaultGame_ShouldBeInitialized");
        
        Assert.IsNotNull(_chessGame);
        Assert.IsNotNull(_chessGame.Board);
        Assert.IsNotNull(_chessGame.CurrentSnapshot);
        Assert.IsNotNull(_chessGame.SnapshotsHistory);
        Assert.AreEqual(0, _chessGame.SnapshotsHistory.Count);
        Assert.AreEqual(ChessTeam.None, _chessGame.Winner);
        Assert.AreNotEqual(0, _chessGame.CurrentSnapshot.SavedPossibleMovements.Count);
        Assert.IsFalse(_chessGame.IsFinished);
    }

    private void AssertPiece<TPiece>(int x, int y, ChessTeam team)
    where TPiece : ChessPiece
    {
        var piece = _chessGame.CurrentSnapshot.GetPiece(new ChessSquare(x, y));
        
        Debug.Log($"Asserting x: {x}, y: {y}, team: {team}");
        Debug.Log($"Info: x: {x}, y: {y}, team: {piece?.Team}");
        
        Assert.IsNotNull(piece);
        Assert.AreEqual(team, piece.Team);
        Assert.IsInstanceOf<TPiece>(piece);
    }
    
    [Test]
    [Order(-int.MaxValue + 1)]
    public void ChessGame_DefaultGame_CheckInitialPieces()
    {
        Debug.Log("ChessGame_DefaultGame_CheckInitialPieces");
        
        AssertPiece<ChessRook>(1, 1, ChessTeam.White);
        AssertPiece<ChessKnight>(2, 1, ChessTeam.White);
        AssertPiece<ChessBishop>(3, 1, ChessTeam.White);
        AssertPiece<ChessQueen>(4, 1, ChessTeam.White);
        AssertPiece<ChessKing>(5, 1, ChessTeam.White);
        AssertPiece<ChessBishop>(6, 1, ChessTeam.White);
        AssertPiece<ChessKnight>(7, 1, ChessTeam.White);
        AssertPiece<ChessRook>(8, 1, ChessTeam.White);
        
        AssertPiece<ChessRook>(1, 8, ChessTeam.Black);
        AssertPiece<ChessKnight>(2, 8, ChessTeam.Black);
        AssertPiece<ChessBishop>(3, 8, ChessTeam.Black);
        AssertPiece<ChessQueen>(4, 8, ChessTeam.Black);
        AssertPiece<ChessKing>(5, 8, ChessTeam.Black);
        AssertPiece<ChessBishop>(6, 8, ChessTeam.Black);
        AssertPiece<ChessKnight>(7, 8, ChessTeam.Black);
        AssertPiece<ChessRook>(8, 8, ChessTeam.Black);
        
        for (int i = 1; i <= 8; i++)
        {
            AssertPiece<ChessPawn>(i, 2, ChessTeam.White);
            AssertPiece<ChessPawn>(i, 7, ChessTeam.Black);
        }

    }
    
}
