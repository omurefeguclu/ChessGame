using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Omedya.ChessLib.Core;

public class ChessGameTests
{
    private ChessGame _chessGame;
    
    [SetUp]
    public void Init()
    {
        _chessGame = ChessGame.CreateDefaultGame();
    }
    
    [Test]
    [Order(-int.MaxValue)]
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
    
    [Test]
    public void ChessGame_MovePiece_ShouldMovePawn()
    {
        var movement = new ChessMovement(new ChessSquare(1, 2), new ChessSquare(1, 3));
        _chessGame.MovePiece(movement);
        
        Assert.AreEqual(1, _chessGame.SnapshotsHistory.Count);
        Assert.AreEqual(ChessTeam.Black, _chessGame.CurrentSnapshot.CurrentTurn);
    }
    
    [Test]
    public void ChessGame_MovePiece_ShouldNotMovePawnBy3()
    {
        var movement = new ChessMovement(new ChessSquare(1, 2), new ChessSquare(1, 5));
        _chessGame.MovePiece(movement);
        
        Assert.AreEqual(0, _chessGame.SnapshotsHistory.Count);
        Assert.AreEqual(ChessTeam.White, _chessGame.CurrentSnapshot.CurrentTurn);
    }
    
    [Test]
    public void ChessGame_MovePiece_PawnCantCaptureIfNoPiece()
    {
        var movement = new ChessMovement(new ChessSquare(1, 2), new ChessSquare(2, 3));
        _chessGame.MovePiece(movement);
        
        Assert.AreEqual(0, _chessGame.SnapshotsHistory.Count);
        Assert.AreEqual(ChessTeam.White, _chessGame.CurrentSnapshot.CurrentTurn);
    }
}
