using System;
using System.Linq;
using Omedya.ChessLib.Core;
using Omedya.ChessLib.Pieces;
using Omedya.ChessLib.SpecialMovements;

namespace Omedya.ChessLib.Util
{
    public class NotationUtil
    {
        public static string GetPieceNotation(ChessPiece piece)
        {
            switch (piece)
            {
                case ChessPawn:
                    return "";
                case ChessKnight:
                    return "N";
                case ChessBishop:
                    return "B";
                case ChessRook:
                    return "R";
                case ChessQueen:
                    return "Q";
                case ChessKing:
                    return "K";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public static string GetSquareColumnNotation(ChessSquare square)
        {
            return $"{(char)('a' + square.X - 1)}";
        }
        public static string GetSquareRowNotation(ChessSquare square)
        {
            return $"{(char)('1' + square.Y - 1)}";
        }
        public static string GetSquareNotation(ChessSquare square)
        {
            return $"{GetSquareColumnNotation(square)}{GetSquareRowNotation(square)}";
        }
        
        public static string GetMovementNotation(ChessMovementInfo movementInfo)
        {
            string notation;
            if(movementInfo.Movement is CastlingMove castlingMove)
            {
                notation = castlingMove.CastlingSide == CastlingSide.KingSide ? "O-O" : "O-O-O";
            }
            else
            {
                // Piece symbol
                notation = GetPieceNotation(movementInfo.MovedPiece);
                
                // Check for possible ambiguity
                var previousSnapshot = movementInfo.BoardSnapshot.PreviousSnapshot;
                if (previousSnapshot is not null)
                {
                    foreach (var possibleMovement in previousSnapshot.SavedPossibleMovements)
                    {
                        if (possibleMovement.End == movementInfo.Movement.End)
                        {
                            var movablePiece = previousSnapshot.GetPiece(possibleMovement.Start);
                            
                            if(movablePiece.GetType() == movementInfo.MovedPiece.GetType() &&
                               movablePiece != movementInfo.MovedPiece &&
                               movablePiece.Team == movementInfo.MovedPiece.Team)
                            {
                                if(possibleMovement.Start.X == movementInfo.Movement.Start.X)
                                {
                                    notation += GetSquareRowNotation(movementInfo.Movement.Start);
                                }
                                else
                                {
                                    notation += GetSquareColumnNotation(movementInfo.Movement.Start);
                                }
                            }
                        }
                    }
                }
                
                // Check for capture
                if (movementInfo.CapturedPiece is not null)
                {
                    if (string.IsNullOrEmpty(notation))
                    {
                        notation += GetSquareColumnNotation(movementInfo.Movement.Start);
                    }
                    notation += "x";
                }
            
                // Add destination square
                notation += GetSquareNotation(movementInfo.Movement.End);
            }
            
            // Check for promotion
            if (movementInfo.Movement is PromotionMove promotionMove)
            {
                notation += $"={GetPieceNotation(promotionMove.PromotionNewPiece)}";
            }

            // Check for check or checkmate (no notation for stalemate)
            var isKingInCheck = MovementValidationUtil.IsKingInCheck(movementInfo.BoardSnapshot, movementInfo.BoardSnapshot.CurrentTurn);
            if (isKingInCheck)
            {
                if (movementInfo.BoardSnapshot.SavedPossibleMovements.Count == 0)
                {
                    notation += "#";
                }
                else
                {
                    notation += "+";
                }
            }
            
            return notation;
        }
    }
}