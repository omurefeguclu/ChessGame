using NUnit.Framework;
using Omedya.ChessLib.Core;
using Omedya.ChessLib.Util;

public class NotationParseTests
{
    private ChessGame _chessGame;
    
    [SetUp]
    public void Init()
    {
        _chessGame = ChessGame.CreateDefaultGame();
    }

    // Simple e4
    [Test]
    public void ChessGame_NotationParser_PawnMovement()
    {
        var movement = NotationUtil.ParseMovement("e4", _chessGame.CurrentSnapshot);
        Assert.NotNull(movement);
        
        Assert.IsTrue(_chessGame.MovePiece(movement));
    }
}