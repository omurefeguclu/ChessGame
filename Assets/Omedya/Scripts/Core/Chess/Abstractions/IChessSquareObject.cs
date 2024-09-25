using Omedya.ChessLib.Core;
using UnityEngine;

namespace Omedya.Scripts.Core.Chess.Abstractions
{
    public interface IChessSquareObject
    {
        Vector3 PiecePosition { get; }
        
        void InitializeSquare(ChessSquare square);
    }
}