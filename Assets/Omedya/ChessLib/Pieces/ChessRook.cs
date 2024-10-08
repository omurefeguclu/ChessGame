﻿using System.Collections.Generic;
using Omedya.ChessLib.Constants;
using Omedya.ChessLib.Core;
using Omedya.ChessLib.Extensions;
using Omedya.ChessLib.Util;

namespace Omedya.ChessLib.Pieces
{
    public class ChessRook : ChessPiece
    {
        public ChessRook(ChessTeam team) : base(team)
        {
        }

        public override IEnumerable<ChessSquare> GetControlledSquares(ChessSquare position, ChessBoardSnapshot boardSnapshot)
        {
            foreach ((int x, int y) direction in MovementConstants.CardinalDirections)
            {
                ChessSquare newSquare = position + direction;
                
                while (boardSnapshot.IsSquareValid(newSquare))
                {
                    yield return newSquare;
                    
                    if(boardSnapshot.IsOccupied(newSquare))
                    {
                        break;
                    }
                    
                    newSquare += direction;
                }
            }
        }

        public override IEnumerable<ChessMovement> GetPossibleMovements(ChessSquare position, ChessBoardSnapshot boardSnapshot)
        {
            foreach (var square in GetControlledSquares(position, boardSnapshot))
            {
                var movement = new ChessMovement(position, square);
                
                if(MovementValidationUtil.ValidateMove(boardSnapshot, movement))
                {
                    yield return movement;
                }
            }
        }
    }
}