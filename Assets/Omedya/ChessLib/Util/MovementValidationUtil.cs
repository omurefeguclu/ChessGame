using System.Linq;
using Omedya.ChessLib.Core;
using Omedya.ChessLib.Extensions;
using Omedya.ChessLib.Pieces;

namespace Omedya.ChessLib.Util
{
    public class MovementValidationUtil
    {
        internal static bool ValidateMove(ChessBoardSnapshot board, ChessMovement movement)
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
            
            if(board.TryGetOccupantTeam(movement.End, out var team) && team == movedPiece.Team)
            {
                return false;
            }

            var rollbackUtil = board.PerformTemporaryMovement(movement);
            // Check if the king is in check
            var isKingInCheck = IsKingInCheck(board, movedPiece.Team);
            
            rollbackUtil.Rollback();
            
            return !isKingInCheck;
        }

        private static bool IsKingInCheck(ChessBoardSnapshot boardSnapshot, ChessTeam kingTeam)
        {
            var kingSquare = boardSnapshot.GetKingSquare(kingTeam);
            
            return IsSquareAttacked(kingSquare, boardSnapshot, kingTeam);
        }
        private static bool IsSquareAttacked(ChessSquare square, ChessBoardSnapshot boardSnapshot,
            ChessTeam friendlyTeam)
        {
            foreach (var movement in boardSnapshot.GetPossibleMovements())
            {
                // TODO: Not attacked if the movement is castling
                if (movement.End == square && boardSnapshot.GetPiece(movement.Start).Team != friendlyTeam)
                {
                    return true;
                }
            }
            
            return false;
        }
    }
}