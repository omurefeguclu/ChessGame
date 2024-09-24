using System.Collections.Generic;
using Omedya.ChessLib.Constants;
using Omedya.ChessLib.Core;
using Omedya.ChessLib.Extensions;
using Omedya.ChessLib.SpecialMovements;
using Omedya.ChessLib.Util;

namespace Omedya.ChessLib.Pieces
{
    public class ChessKing : ChessPiece
    {
        public ChessKing(ChessTeam team) : base(team)
        {
        }

        private static bool CanPerformKingMovementForCastling(ChessSquare position, ChessBoardSnapshot boardSnapshot,
            (int x, int y) castlingDirection)

        {
            var kingFirstSquare = new ChessMovement(position, position + castlingDirection);
            var movement2 = new ChessMovement(position, position + castlingDirection + castlingDirection);

            return boardSnapshot.IsSquareValid(kingFirstSquare.End) &&
                   MovementValidationUtil.ValidateMove(boardSnapshot, kingFirstSquare)
                   && boardSnapshot.IsSquareValid(movement2.End) &&
                   MovementValidationUtil.ValidateMove(boardSnapshot, movement2);
        }
        
        public override IEnumerable<ChessMovement> GetPossibleMovements(ChessSquare position, ChessBoardSnapshot boardSnapshot)
        {
            foreach ((int x, int y) direction in MovementConstants.AllDirections)
            {
                ChessSquare newSquare = position + direction;

                if (boardSnapshot.IsSquareValid(newSquare))
                {
                    var movement = new ChessMovement(position, newSquare);

                    if (MovementValidationUtil.ValidateMove(boardSnapshot, movement))
                    {
                        yield return movement;
                    }
                }
            }
            
            // Castling
            
            // King side
            if(boardSnapshot.CanCastle[(Team, CastlingSide.KingSide)])
            {
                var rookSquare = Team == ChessTeam.White ? new ChessSquare(8, 1) : new ChessSquare(8, 8);
                // For future different board shapes
                var castlingDirection = (1, 0);
                
                if (CanPerformKingMovementForCastling(position, boardSnapshot, castlingDirection))
                {
                    yield return new CastlingMove(position, position + castlingDirection + castlingDirection,
                        rookSquare);
                }
            }
            // Queen side
            if(boardSnapshot.CanCastle[(Team, CastlingSide.QueenSide)])
            {
                var rookSquare = Team == ChessTeam.White ? new ChessSquare(1, 1) : new ChessSquare(1, 8);
                // For future different board shapes
                var castlingDirection = (-1, 0);
                
                if (CanPerformKingMovementForCastling(position, boardSnapshot, castlingDirection))
                {
                    yield return new CastlingMove(position, position + castlingDirection + castlingDirection,
                        rookSquare);
                }
            }
            
            
        }
    }
}