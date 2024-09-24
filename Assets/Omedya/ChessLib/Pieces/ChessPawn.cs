using System.Collections.Generic;
using Omedya.ChessLib.Core;
using Omedya.ChessLib.Extensions;
using Omedya.ChessLib.SpecialMovements;
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
                yield return new ChessMovement(position, newSquare);

                // Double move
                var isFirstMove = position.Y == pawnRow;
                if (isFirstMove)
                {
                    newSquare = position + direction + direction;
            
                    if (boardSnapshot.IsSquareValid(newSquare) && !boardSnapshot.IsOccupied(newSquare))
                    {
                        yield return new ChessMovement(position, newSquare);
                    }    
                }
            }
            
            
            // Capture moves

            newSquare = position + direction + captureSideDirection;
            if (boardSnapshot.IsSquareValid(newSquare) && boardSnapshot.TryGetOccupantTeam(newSquare, out var occupantTeam) &&
                occupantTeam != Team) 
            {
                yield return new ChessMovement(position, newSquare);
            }
            
            newSquare = position + direction - captureSideDirection;
            if (boardSnapshot.IsSquareValid(newSquare) && boardSnapshot.TryGetOccupantTeam(newSquare, out occupantTeam) &&
                occupantTeam != Team) 
            {
                yield return new ChessMovement(position, newSquare);
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
                    
                    yield return new EnPassantMove(position, newSquare, capturedSquare);
                }
                
                capturedSquare = position - captureSideDirection;
                if (capturedSquare == lastMovement.End)
                {
                    newSquare = capturedSquare - captureSideDirection;

                    yield return new EnPassantMove(position, newSquare, capturedSquare);
                }
            }
        }
    }
}