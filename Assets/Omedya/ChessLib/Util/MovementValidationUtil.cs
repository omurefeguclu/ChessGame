using Omedya.ChessLib.Core;
using Omedya.ChessLib.Extensions;
using Omedya.ChessLib.Pieces;

namespace Omedya.ChessLib.Util
{
    public class MovementValidationUtil
    {
        internal static bool ValidateMove(ChessBoard board, ChessMovement movement)
        {
            if (!board.IsSquareValid(movement.Start) || !board.IsSquareValid(movement.End))
            {
                return false;
            }

            var movedPiece = board.GetPiece(movement.Start);
            if (movedPiece is null)
            {
                return false;
            }
            
            // Implement
            if(board.GetPiece(movement.End) is ChessKing)
            {
                return false;
            }
            
            if(board.TryGetOccupantTeam(movement.End, out var team) && team == movedPiece.Team)
            {
                return false;
            }

            var rollbackUtil = board.PerformMovementTemp(movement);
            

            return true;
        }
    }
}