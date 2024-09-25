using Omedya.ChessLib.Core;
using Omedya.Scripts.Core.Chess.Abstractions;
using UnityEngine;

namespace Omedya.Scripts.Core.Chess.Components
{
    public class ChessPieceUI : MonoBehaviour, IChessPieceObject
    {
        public ChessPiece Piece { get; private set; }
        
        public void InitializePiece(ChessPiece piece)
        {
            Piece = piece;
            
            // Add related sprite to the piece ui
        }
    }
}