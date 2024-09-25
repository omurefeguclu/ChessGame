using System.Collections.Generic;
using Omedya.ChessLib.Core;
using Omedya.ChessLib.Extensions;
using Omedya.ChessLib.SpecialMovements;
using Omedya.ChessLib.Util;
using UnityEngine;

namespace Omedya.ChessLib.Pieces
{
    public class ChessPawn : ChessPiece
    {
        public ChessPawn(ChessTeam team) : base(team)
        {
        }

        public override IEnumerable<ChessSquare> GetControlledSquares(ChessSquare position, ChessBoardSnapshot boardSnapshot)
        {
            (int x, int y) captureSideDirection = (1, 0);
            (int x, int y) direction = Team == ChessTeam.White ? (0, 1) : (0, -1);
            
            var newSquare = position + direction + captureSideDirection;
            if (boardSnapshot.IsSquareValid(newSquare))
            {
                yield return newSquare;
            }
            
            newSquare = position + direction - captureSideDirection;
            if (boardSnapshot.IsSquareValid(newSquare)) 
            {
                yield return newSquare;
            }
            
            // Important: If this method will be used in an AI implementation, this method doesn't take en passant into account
            
        }

        public override IEnumerable<ChessMovement> GetPossibleMovements(ChessSquare position, ChessBoardSnapshot boardSnapshot)
        {
            (int x, int y) direction = Team == ChessTeam.White ? (0, 1) : (0, -1);
            // Creating the capture side variable to implement in future different board shapes
            (int x, int y) captureSideDirection = (1, 0);
            
            int pawnRow = Team == ChessTeam.White ? 2 : 7;
            int promotionRow = Team == ChessTeam.White ? 8 : 1;
            
            ChessSquare newSquare = position + direction;

            if (boardSnapshot.IsSquareValid(newSquare) && !boardSnapshot.IsOccupied(newSquare))
            {
                var movement = newSquare.Y == promotionRow
                    ? new PromotionMove(position, newSquare)
                    : new ChessMovement(position, newSquare);

                if(MovementValidationUtil.ValidateMove(boardSnapshot, movement))
                {
                    yield return movement;

                    // Double move
                    var isFirstMove = position.Y == pawnRow;
                    if (isFirstMove)
                    {
                        newSquare = position + direction + direction;
            
                        if (boardSnapshot.IsSquareValid(newSquare) && !boardSnapshot.IsOccupied(newSquare))
                        {
                            var doubleMove = new ChessMovement(position, newSquare);
                            
                            if(MovementValidationUtil.ValidateMove(boardSnapshot, doubleMove))
                            {
                                yield return doubleMove;
                            }
                        }    
                    }
                }
                
                
            }
            
            
            // Capture moves
            foreach (var square in GetControlledSquares(position, boardSnapshot))
            {
                var movement = square.Y == promotionRow
                    ? new PromotionMove(position, square)
                    : new ChessMovement(position, square);

                if (boardSnapshot.TryGetOccupantTeam(square, out var team) && team != Team &&
                    MovementValidationUtil.ValidateMove(boardSnapshot, movement))
                {
                    yield return movement;
                }
            }

            
            // En passant
            var lastMovement = boardSnapshot.LastMovement;
            if (lastMovement?.MovedPiece is ChessPawn lastPawn &&
                lastPawn.Team != Team && lastMovement.Movement.End.Y == position.Y)
            {
                var displacement = lastMovement.Movement.End - lastMovement.Movement.Start;
                var isDoubleMove = (displacement.x == 0 && Mathf.Abs(displacement.y) == 2) ||
                                   // This is a special case for future board shapes
                                   (displacement.y == 0 && Mathf.Abs(displacement.x) == 2);

                if (isDoubleMove)
                {
                    var offset = lastMovement.Movement.End - position;
                    if (offset == captureSideDirection || offset == (-captureSideDirection.x, -captureSideDirection.y))
                    {
                        newSquare = position + direction + offset;
                        
                        var enPassantMove = new EnPassantMove(position, newSquare, lastMovement.Movement.End);
                        if(MovementValidationUtil.ValidateMove(boardSnapshot, enPassantMove))
                        {
                            yield return enPassantMove;
                        }
                    }
                }
            }
            
            
        }
        
    }
}