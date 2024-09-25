using Omedya.ChessLib.Core;

namespace Omedya.Scripts.Core.Chess.Abstractions
{
    public interface IChessBoardObject
    {
        void CreateBoard(ChessBoard board);
        
        void UpdateBoardSnapshot(ChessBoardSnapshot snapshot);
    }
}