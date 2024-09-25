using NUnit.Framework;
using Omedya.ChessLib.Core;

public class NotationWriteTests
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

    [Test]
    public void ChessGame_Notation_Checkmate()
    {
        var movement = new ChessMovement(new ChessSquare(5, 2), new ChessSquare(5, 4));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        Assert.AreEqual("e4", _chessGame.CurrentSnapshot.LastMovement.ToString());
        
        movement = new ChessMovement(new ChessSquare(7, 7), new ChessSquare(7, 5));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        Assert.AreEqual("g5", _chessGame.CurrentSnapshot.LastMovement.ToString());
        
        movement = new ChessMovement(new ChessSquare(4, 2), new ChessSquare(4, 4));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        Assert.AreEqual("d4", _chessGame.CurrentSnapshot.LastMovement.ToString());
        
        movement = new ChessMovement(new ChessSquare(6, 7), new ChessSquare(6, 6));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        Assert.AreEqual("f6", _chessGame.CurrentSnapshot.LastMovement.ToString());
        
        movement = new ChessMovement(new ChessSquare(4, 1), new ChessSquare(8, 5));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        Assert.AreEqual("Qh5#", _chessGame.CurrentSnapshot.LastMovement.ToString());
        
        Assert.IsTrue(_chessGame.IsFinished);
        Assert.AreEqual(ChessTeam.White, _chessGame.Winner);
    }

    [Test]
    public void ChessGame_Notation_Ambiguity()
    {
        // e4 d5 Nf3 d4 Nc3 h5 Nb5 h6 Nbxd4
        var movement = new ChessMovement(new ChessSquare(5, 2), new ChessSquare(5, 4));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        Assert.AreEqual("e4", _chessGame.CurrentSnapshot.LastMovement.ToString());
        
        movement = new ChessMovement(new ChessSquare(4, 7), new ChessSquare(4, 5));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        Assert.AreEqual("d5", _chessGame.CurrentSnapshot.LastMovement.ToString());
        
        movement = new ChessMovement(new ChessSquare(7, 1), new ChessSquare(6, 3));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        Assert.AreEqual("Nf3", _chessGame.CurrentSnapshot.LastMovement.ToString());
        
        movement = new ChessMovement(new ChessSquare(4, 5), new ChessSquare(4, 4));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        Assert.AreEqual("d4", _chessGame.CurrentSnapshot.LastMovement.ToString());
        
        movement = new ChessMovement(new ChessSquare(2, 1), new ChessSquare(3, 3));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        Assert.AreEqual("Nc3", _chessGame.CurrentSnapshot.LastMovement.ToString());
        
        movement = new ChessMovement(new ChessSquare(8, 7), new ChessSquare(8, 5));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        Assert.AreEqual("h5", _chessGame.CurrentSnapshot.LastMovement.ToString());
        
        movement = new ChessMovement(new ChessSquare(3, 3), new ChessSquare(2, 5));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        Assert.AreEqual("Nb5", _chessGame.CurrentSnapshot.LastMovement.ToString());
        
        movement = new ChessMovement(new ChessSquare(8, 5), new ChessSquare(8, 4));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        Assert.AreEqual("h4", _chessGame.CurrentSnapshot.LastMovement.ToString());
        
        movement = new ChessMovement(new ChessSquare(2, 5), new ChessSquare(4, 4));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        Assert.AreEqual("Nbxd4", _chessGame.CurrentSnapshot.LastMovement.ToString());
        
        
    }
    
    [Test]
    public void ChessGame_Notation_Ambiguity_NotOnPawn()
    {
        // e4
        var movement = new ChessMovement(new ChessSquare(5, 2), new ChessSquare(5, 4));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        Assert.AreEqual("e4", _chessGame.CurrentSnapshot.LastMovement.ToString());
        
        // d5
        movement = new ChessMovement(new ChessSquare(4, 7), new ChessSquare(4, 5));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        Assert.AreEqual("d5", _chessGame.CurrentSnapshot.LastMovement.ToString());
        
        // c4
        movement = new ChessMovement(new ChessSquare(3, 2), new ChessSquare(3, 4));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        Assert.AreEqual("c4", _chessGame.CurrentSnapshot.LastMovement.ToString());
        
        // Nf6
        movement = new ChessMovement(new ChessSquare(7, 8), new ChessSquare(6, 6));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        Assert.AreEqual("Nf6", _chessGame.CurrentSnapshot.LastMovement.ToString());
        
        // cxd5
        movement = new ChessMovement(new ChessSquare(3, 4), new ChessSquare(4, 5));
        Assert.IsTrue(_chessGame.MovePiece(movement));
        Assert.AreEqual("cxd5", _chessGame.CurrentSnapshot.LastMovement.ToString());
    }
}