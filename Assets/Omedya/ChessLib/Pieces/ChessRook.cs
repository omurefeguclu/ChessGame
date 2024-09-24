using System.Collections.Generic;
using Omedya.ChessLib.Constants;
using Omedya.ChessLib.Core;
using Omedya.ChessLib.Extensions;

namespace Omedya.ChessLib.Pieces
{
    public class ChessRook : ChessPiece
    {
        public ChessRook(ChessTeam team) : base(team)
        {
        }

        public override IEnumerable<ChessMovement> GetPossibleMovements(ChessSquare position, ChessGame game,
            ChessBoard board)
        {
            foreach ((int x, int y) direction in MovementConstants.CardinalDirections)
            {
                ChessSquare newSquare = position + direction;
                
                while (board.IsSquareValid(newSquare))
                {
                    
                    if(board.TryGetOccupantTeam(newSquare, out var team))
                    {
                        
                    }
                    
                    yield return new ChessMovement(position, newSquare);
                    newSquare += direction;
                }
            }
        }
    }
}