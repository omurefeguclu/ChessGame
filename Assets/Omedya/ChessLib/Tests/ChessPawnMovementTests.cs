using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Omedya.ChessLib.Core;
using Omedya.ChessLib.Pieces;
using Omedya.ChessLib.SpecialMovements;
using UnityEngine;

public class ChessPawnMovementTests
{
    
    private ChessGame _chessGame;
    
    [SetUp]
    public void Init()
    {
        _chessGame = ChessGame.CreateDefaultGame();
    }

    #region Movement
    
    // Pawn movements
    [Test]
    public void ChessGame_MovePiece_ShouldMovePawn()
    {
        var movement = new ChessMovement(new ChessSquare(1, 2), new ChessSquare(1, 3));
        _chessGame.MovePiece(movement);
        
        Assert.AreEqual(1, _chessGame.SnapshotsHistory.Count);
        Assert.AreEqual(ChessTeam.Black, _chessGame.CurrentSnapshot.CurrentTurn);
    }

    [Test]
    public void ChessGame_MovePiece_ShouldDoubleMovePawn()
    {
        var movement = new ChessMovement(new ChessSquare(1, 2), new ChessSquare(1, 4));
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
    
    #endregion
    
    #region Capture tests
    
    [Test]
    public void ChessGame_MovePiece_PawnCantCaptureIfNoPiece()
    {
        var movement = new ChessMovement(new ChessSquare(1, 2), new ChessSquare(2, 3));
        Assert.IsFalse(_chessGame.MovePiece(movement));
        
        Assert.AreEqual(ChessTeam.White, _chessGame.CurrentSnapshot.CurrentTurn);
    }

    [Test]
    public void ChessGame_MovePiece_PawnCanCapture()
    {
        var movement = new ChessMovement(new ChessSquare(1, 2), new ChessSquare(1, 4));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        
        movement = new ChessMovement(new ChessSquare(2, 7), new ChessSquare(2, 5));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        
        movement = new ChessMovement(new ChessSquare(1, 4), new ChessSquare(2, 5));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        
        Assert.AreEqual(ChessTeam.Black, _chessGame.CurrentSnapshot.CurrentTurn);
    }
    
    #endregion

    #region En passant test

    [Test]
    public void ChessGame_MovePiece_PawnInvalidEnPassant()
    {
        var movement = new ChessMovement(new ChessSquare(1, 2), new ChessSquare(1, 4));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        
        movement = new ChessMovement(new ChessSquare(2, 7), new ChessSquare(2, 5));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        
        movement = new ChessMovement(new ChessSquare(1, 4), new ChessSquare(1, 5));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        
        movement = new ChessMovement(new ChessSquare(2, 5), new ChessSquare(2, 4));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        
        movement = new ChessMovement(new ChessSquare(1, 5), new ChessSquare(2, 6));
        Assert.IsFalse(_chessGame.MovePiece(movement));
        
        Assert.AreEqual(ChessTeam.White, _chessGame.CurrentSnapshot.CurrentTurn);
    }
    [Test]
    public void ChessGame_MovePiece_PawnInvalidEnPassant2()
    {
        var movement = new ChessMovement(new ChessSquare(2, 2), new ChessSquare(2, 4));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        
        movement = new ChessMovement(new ChessSquare(7, 7), new ChessSquare(7, 5));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        
        movement = new ChessMovement(new ChessSquare(2, 4), new ChessSquare(2, 5));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        
        movement = new ChessMovement(new ChessSquare(1, 7), new ChessSquare(1, 5));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        
        movement = new ChessMovement(new ChessSquare(2, 5), new ChessSquare(3, 6));
        Assert.IsFalse(_chessGame.MovePiece(movement));
        
        Assert.AreEqual(ChessTeam.White, _chessGame.CurrentSnapshot.CurrentTurn);
    }
    
    [Test]
    public void ChessGame_MovePiece_PawnCaptureEnPassant()
    {
        var movement = new ChessMovement(new ChessSquare(2, 2), new ChessSquare(2, 4));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        
        movement = new ChessMovement(new ChessSquare(7, 7), new ChessSquare(7, 5));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        
        movement = new ChessMovement(new ChessSquare(2, 4), new ChessSquare(2, 5));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        
        movement = new ChessMovement(new ChessSquare(1, 7), new ChessSquare(1, 5));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        
        movement = new ChessMovement(new ChessSquare(2, 5), new ChessSquare(1, 6));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        
        Assert.AreEqual(ChessTeam.Black, _chessGame.CurrentSnapshot.CurrentTurn);
    }

    #endregion
 
    #region Promotion tests
    
    [Test]
    public void ChessGame_MovePiece_PawnPromotion()
    {
        var movement = new ChessMovement(new ChessSquare(1, 2), new ChessSquare(1, 4));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        
        movement = new ChessMovement(new ChessSquare(7, 7), new ChessSquare(7, 5));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        
        movement = new ChessMovement(new ChessSquare(1, 4), new ChessSquare(1, 5));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        
        movement = new ChessMovement(new ChessSquare(7, 5), new ChessSquare(7, 4));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        
        movement = new ChessMovement(new ChessSquare(1, 5), new ChessSquare(1, 6));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        
        movement = new ChessMovement(new ChessSquare(7, 4), new ChessSquare(7, 3));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        
        movement = new ChessMovement(new ChessSquare(1, 6), new ChessSquare(2, 7));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        
        movement = new ChessMovement(new ChessSquare(7, 3), new ChessSquare(8, 2));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        
        var promotionMovement = new PromotionMove(new ChessSquare(2, 7), new ChessSquare(1, 8));
        promotionMovement.SelectPromotionPiece(new ChessQueen(ChessTeam.White));
        Assert.IsTrue(_chessGame.MovePiece(promotionMovement));
        
        // Check if the promotion is successful
        var piece = _chessGame.CurrentSnapshot.GetPiece(new ChessSquare(1, 8));
        Assert.IsNotNull(piece);
        Assert.IsInstanceOf<ChessQueen>(piece);
        
        Assert.AreEqual(ChessTeam.Black, _chessGame.CurrentSnapshot.CurrentTurn);
    }
    
    #endregion
}
