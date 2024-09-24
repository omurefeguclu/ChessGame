using System.Collections.Generic;
using Omedya.ChessLib.Constants;
using Omedya.ChessLib.Core;
using Omedya.ChessLib.Extensions;
using Omedya.ChessLib.Util;

namespace Omedya.ChessLib.Pieces
{
    public class ChessQueen : ChessPiece
    {
        public ChessQueen(ChessTeam team) : base(team)
        {
        }

        public override IEnumerable<ChessMovement> GetPossibleMovements(ChessSquare position, ChessBoardSnapshot boardSnapshot)
        {
            foreach ((int x, int y) direction in MovementConstants.AllDirections)
            {
                ChessSquare newSquare = position + direction;
                
                while (boardSnapshot.IsSquareValid(newSquare))
                {
                    var movement = new ChessMovement(position, newSquare);
                    
                    if(MovementValidationUtil.ValidateMove(boardSnapshot, movement))
                    {
                        yield return movement;
                    }
                    
                    if(boardSnapshot.TryGetOccupantTeam(newSquare, out var team))
                    {
                        break;
                    }
                    
                    newSquare += direction;
                }
            }
        }
    }
}