using System.Collections.Generic;
using Omedya.ChessLib.Constants;
using Omedya.ChessLib.Core;
using Omedya.ChessLib.Extensions;
using Omedya.ChessLib.Util;

namespace Omedya.ChessLib.Pieces
{
    public class ChessKnight : ChessPiece
    {
        public ChessKnight(ChessTeam team) : base(team)
        {
        }

        public override IEnumerable<ChessMovement> GetPossibleMovements(ChessSquare position, ChessBoardSnapshot boardSnapshot)
        {
            foreach ((int x, int y) direction in MovementConstants.KnightMovements)
            {
                ChessSquare newSquare = position + direction;
                
                if(boardSnapshot.IsSquareValid(newSquare))
                {
                    var movement = new ChessMovement(position, newSquare);
                    
                    if(MovementValidationUtil.ValidateMove(boardSnapshot, movement))
                    {
                        yield return movement;
                    }
                }
            }
        }
    }    
}
