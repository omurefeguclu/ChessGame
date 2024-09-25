using NUnit.Framework;
using Omedya.ChessLib.Core;

public class NotationTests
{
    
    private ChessGame _chessGame;
    
    [SetUp]
    public void Init()
    {
        _chessGame = ChessGame.CreateDefaultGame();
    }

    
    // Notation Util Tests
    [Test]
    public void ChessGame_Notation_PawnMovement()
    {
        var movement = new ChessMovement(new ChessSquare(5, 2), new ChessSquare(5, 4));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        
        Assert.AreEqual(1, _chessGame.SnapshotsHistory.Count);
        Assert.AreEqual(ChessTeam.Black, _chessGame.CurrentSnapshot.CurrentTurn);
        
        var lastMovement = _chessGame.CurrentSnapshot.LastMovement;
        Assert.NotNull(lastMovement);
        Assert.AreEqual("e4", lastMovement.ToString());
    }
    
    [Test]
    public void ChessGame_Notation_KnightMovement()
    {
        var movement = new ChessMovement(new ChessSquare(2, 1), new ChessSquare(3, 3));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        
        Assert.AreEqual(1, _chessGame.SnapshotsHistory.Count);
        Assert.AreEqual(ChessTeam.Black, _chessGame.CurrentSnapshot.CurrentTurn);
        
        var lastMovement = _chessGame.CurrentSnapshot.LastMovement;
        Assert.AreEqual("Nc3", lastMovement.ToString());
    }

    [Test]
    public void ChessGame_Notation_PawnCapture()
    {
        var movement = new ChessMovement(new ChessSquare(5, 2), new ChessSquare(5, 4));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        
        movement = new ChessMovement(new ChessSquare(4, 7), new ChessSquare(4, 5));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        
        movement = new ChessMovement(new ChessSquare(5, 4), new ChessSquare(4, 5));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        
        Assert.AreEqual(3, _chessGame.SnapshotsHistory.Count);
        Assert.AreEqual(ChessTeam.Black, _chessGame.CurrentSnapshot.CurrentTurn);
        
        var lastMovement = _chessGame.CurrentSnapshot.LastMovement;
        Assert.AreEqual("exd5", lastMovement.ToString());
    }

    [Test]
    public void ChessGame_Notation_QueenCapture()
    {
        var movement = new ChessMovement(new ChessSquare(5, 2), new ChessSquare(5, 4));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        
        movement = new ChessMovement(new ChessSquare(4, 7), new ChessSquare(4, 5));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        
        movement = new ChessMovement(new ChessSquare(5, 4), new ChessSquare(4, 5));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        
        movement = new ChessMovement(new ChessSquare(4, 8), new ChessSquare(4, 5));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        
        Assert.AreEqual(4, _chessGame.SnapshotsHistory.Count);
        Assert.AreEqual(ChessTeam.White, _chessGame.CurrentSnapshot.CurrentTurn);
        
        var lastMovement = _chessGame.CurrentSnapshot.LastMovement;
        Assert.AreEqual("Qxd5", lastMovement.ToString());
    }

    [Test]
    public void ChessGame_Notation_TakesAndCheck()
    {
        var movement = new ChessMovement(new ChessSquare(5, 2), new ChessSquare(5, 4));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        
        movement = new ChessMovement(new ChessSquare(4, 7), new ChessSquare(4, 5));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        
        movement = new ChessMovement(new ChessSquare(5, 4), new ChessSquare(4, 5));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        
        movement = new ChessMovement(new ChessSquare(4, 8), new ChessSquare(4, 5));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        
        movement = new ChessMovement(new ChessSquare(8, 2), new ChessSquare(8, 4));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        
        movement = new ChessMovement(new ChessSquare(4, 5), new ChessSquare(4, 2));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        
        Assert.AreEqual(6, _chessGame.SnapshotsHistory.Count);
        Assert.AreEqual(ChessTeam.White, _chessGame.CurrentSnapshot.CurrentTurn);
        
        var lastMovement = _chessGame.CurrentSnapshot.LastMovement;
        Assert.AreEqual("Qxd2+", lastMovement.ToString());
    }
}