using System.Linq;
using Omedya.ChessLib.Core;
using Omedya.ChessLib.Extensions;
using Omedya.ChessLib.Pieces;
using UnityEngine;

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

            if (board.TryGetOccupantTeam(movement.End, out var team) && team == movedPiece.Team)
            {
                return false;
            }

            var rollbackUtil = board.PerformTemporaryMovement(movement);
            // Check if the king is in check
            var isKingInCheck = IsKingInCheck(board, movedPiece.Team);

            rollbackUtil.Rollback();

            return !isKingInCheck;
        }

        public static bool IsKingInCheck(ChessBoardSnapshot boardSnapshot, ChessTeam kingTeam)
        {
            var kingSquare = boardSnapshot.GetKingSquare(kingTeam);
            
            return IsSquareAttacked(kingSquare, boardSnapshot, kingTeam);
        }
        private static bool IsSquareAttacked(ChessSquare square, ChessBoardSnapshot boardSnapshot,
            ChessTeam friendlyTeam)
        {
            foreach (var controlledSquare in boardSnapshot.GetAttackedSquares(friendlyTeam))
            {
                if (controlledSquare == square && boardSnapshot.GetPiece(controlledSquare).Team == friendlyTeam)
                {
                    return true;
                }
            }
            
            return false;
        }
    }
}