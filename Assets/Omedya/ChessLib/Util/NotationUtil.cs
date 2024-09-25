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
                    
                    foreach (var possibleMovement in previousSnapshot.GetPossibleMovementsByEnd(movementInfo.Movement.End))
                    {
                        var movablePiece = previousSnapshot.GetPiece(possibleMovement.Start);

                        // Check if the piece is the same type, same team and not the same piece
                        if (movablePiece.GetType() != movementInfo.MovedPiece.GetType() ||
                            movablePiece == movementInfo.MovedPiece ||
                            movablePiece.Team != movementInfo.MovedPiece.Team) continue;
                        
                        // Decide if column or row will solve the ambiguity
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
                if (!movementInfo.BoardSnapshot.AnyMovesAvailable)
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
        
        public static ChessMovement ParseMovement(string movementNotation, ChessBoardSnapshot boardSnapshot)
        {
            var possibleMovements = boardSnapshot.SavedPossibleMovements;
            
            // Check for castling
            if (movementNotation == "O-O")
            {
                return possibleMovements.First(m => m is CastlingMove castlingMove && castlingMove.CastlingSide == CastlingSide.KingSide);
            }
            else if (movementNotation == "O-O-O")
            {
                return possibleMovements.First(m => m is CastlingMove castlingMove && castlingMove.CastlingSide == CastlingSide.QueenSide);
            }
            else
            {
                // Remove +, #, x from the notation
                var processingNotation = movementNotation.Replace("+", "").Replace("#", "").Replace("x", "");
                
                // Check for promotion
                var promotionIndex = processingNotation.IndexOf('=');
                
                // Promotion Movement
                if(promotionIndex != -1)
                {
                    processingNotation = processingNotation.Remove(promotionIndex);
                    
                    var promotionPiece = processingNotation[^1];
                    ChessPiece promotionNewPiece = promotionPiece switch
                    {
                        'N' => new ChessKnight(boardSnapshot.CurrentTurn),
                        'B' => new ChessBishop(boardSnapshot.CurrentTurn),
                        'R' => new ChessRook(boardSnapshot.CurrentTurn),
                        'Q' => new ChessQueen(boardSnapshot.CurrentTurn),
                        _ => throw new ArgumentOutOfRangeException()
                    };
                    
                    processingNotation = processingNotation.Remove(processingNotation.Length - 1);
                    
                    var startColumn = processingNotation[0] - 'a' + 1;
                    var endSquare = new ChessSquare(processingNotation[^2] - 'a' + 1, processingNotation[^1] - '1' + 1);

                    
                    foreach (var possibleMovement in possibleMovements)
                    {
                        if (possibleMovement.Start.X != startColumn || possibleMovement.End != endSquare ||
                            possibleMovement is not PromotionMove promotionMove)
                            continue;
                        
                        promotionMove.SelectPromotionPiece(promotionNewPiece);
                            
                        return promotionMove;
                    }
                }
                else
                {
                    // x is removed, so if the move was take input should be something like cb8 (if pawn), otherwise input should be like b8 (if pawn), or Nf3 (if knight)
                    var endSquare = new ChessSquare(processingNotation[^2] - 'a' + 1, processingNotation[^1] - '1' + 1);
                    
                    // if first letter is between 'a' and 'h' it is a pawn move, otherwise it is a piece move
                    if (processingNotation[0] >= 'a' && processingNotation[0] <= 'h')
                    {
                        processingNotation = processingNotation.Insert(0, "P");
                    }
                    
                    var pieceTypeCharacter = processingNotation[0];
                    Func<ChessPiece, bool> typeCheck = pieceTypeCharacter switch
                    {
                        'N' => piece => piece is ChessKnight,
                        'B' => piece => piece is ChessBishop,
                        'R' => piece => piece is ChessRook,
                        'Q' => piece => piece is ChessQueen,
                        'K' => piece => piece is ChessKing,
                        'P' => piece => piece is ChessPawn,
                        _ => throw new ArgumentException()
                    };
                    Func<ChessMovement, bool> ambiguityCheck;
                    
                    // Check for ambiguity (if there are multiple pieces that can move to the same square)
                    if (processingNotation.Length == 4)
                    {
                        // Either column or row is specified
                        var ambiguityCharacter = processingNotation[1];
                        switch (ambiguityCharacter)
                        {
                            case >= 'a' and <= 'h':
                                var ambiguityColumn = ambiguityCharacter - 'a' + 1;
                                
                                ambiguityCheck = movement => movement.Start.X == ambiguityColumn;
                                break;
                            case >= '1' and <= '8':
                                var ambiguityRow = ambiguityCharacter - '1' + 1;
                                
                                ambiguityCheck = movement => movement.Start.Y == ambiguityRow;
                                break;
                            default:
                                throw new Exception("Unexpected ambiguity character");
                        }
                        
                    }
                    else
                    {
                        ambiguityCheck = movement => true;
                    }

                    foreach (var possibleMovement in possibleMovements)
                    {
                        var piece = boardSnapshot.GetPiece(possibleMovement.Start);
                        if (possibleMovement.End != endSquare || !typeCheck(piece) ||
                            !ambiguityCheck(possibleMovement))
                            continue;

                        return possibleMovement;
                    }

                }
                
                throw new Exception("Movement invalid");
            }
            
        }
    }
}