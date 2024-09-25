﻿using Omedya.ChessLib.Util;

namespace Omedya.ChessLib.Core
{
    public class ChessMovementInfo
    {
        public ChessMovement Movement { get; }
        
        
        public ChessPiece MovedPiece { get; }
        public ChessPiece CapturedPiece { get; }
        public ChessBoardSnapshot BoardSnapshot { get; }
        
        public ChessMovementInfo(ChessMovement movement, ChessBoardSnapshot boardSnapshot)
        {
            Movement = movement;
            
            BoardSnapshot = boardSnapshot;
            MovedPiece = boardSnapshot.GetPiece(movement.Start);
            CapturedPiece = boardSnapshot.GetPiece(movement.End);
        }
        
        
        public override string ToString()
        {
            return NotationUtil.GetMovementNotation(this);
        }
    }
}