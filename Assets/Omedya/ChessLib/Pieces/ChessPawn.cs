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


        public override IEnumerable<ChessMovement> GetPossibleMovements(ChessSquare position, ChessBoardSnapshot boardSnapshot)
        {
            (int x, int y) direction = Team == ChessTeam.White ? (0, 1) : (0, -1);
            // Creating the capture side variable to implement in future different board shapes
            (int x, int y) captureSideDirection = (1, 0);
            
            int pawnRow = Team == ChessTeam.White ? 2 : 7;
            ChessSquare newSquare = position + direction;

            if (boardSnapshot.IsSquareValid(newSquare) && !boardSnapshot.IsOccupied(newSquare))
            {
                var movement = new ChessMovement(position, newSquare);

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

            newSquare = position + direction + captureSideDirection;
            if (boardSnapshot.IsSquareValid(newSquare) && boardSnapshot.TryGetOccupantTeam(newSquare, out var occupantTeam) &&
                occupantTeam != Team)
            {
                var movement = new ChessMovement(position, newSquare);
                
                if(MovementValidationUtil.ValidateMove(boardSnapshot, movement))
                {
                    yield return movement;
                }
            }
            
            newSquare = position + direction - captureSideDirection;
            if (boardSnapshot.IsSquareValid(newSquare) && boardSnapshot.TryGetOccupantTeam(newSquare, out occupantTeam) &&
                occupantTeam != Team) 
            {
                var movement = new ChessMovement(position, newSquare);
                
                if(MovementValidationUtil.ValidateMove(boardSnapshot, movement))
                {
                    yield return movement;
                }
            }
            
            // En passant
            var lastMovement = boardSnapshot.LastMovement;
            if (lastMovement is not null && lastMovement.StartPiece is ChessPawn lastPawn &&
                lastPawn.Team != Team && lastMovement.Start.X == lastMovement.End.X &&
                Mathf.Abs(lastMovement.End.Y - lastMovement.Start.Y) == 2)
            {
                var capturedSquare = position + captureSideDirection;
                if (capturedSquare == lastMovement.End)
                {
                    newSquare = capturedSquare + direction;    
                    
                    var enPassantMove = new EnPassantMove(position, newSquare, capturedSquare);
                    if(MovementValidationUtil.ValidateMove(boardSnapshot, enPassantMove))
                    {
                        yield return enPassantMove;
                    }
                }
                
                capturedSquare = position - captureSideDirection;
                if (capturedSquare == lastMovement.End)
                {
                    newSquare = capturedSquare - captureSideDirection;

                    var enPassantMove = new EnPassantMove(position, newSquare, capturedSquare);
                    if(MovementValidationUtil.ValidateMove(boardSnapshot, enPassantMove))
                    {
                        yield return enPassantMove;
                    }
                }
            }
        }
    }
}