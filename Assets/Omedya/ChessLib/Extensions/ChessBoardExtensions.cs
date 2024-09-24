using Omedya.ChessLib.Core;

namespace Omedya.ChessLib.Extensions
{
    public static class ChessBoardExtensions
    {
        internal static bool IsOccupied(this ChessBoardSnapshot board, ChessSquare square)
        {
            return board.GetPiece(square) is not null;
        }
        internal static bool TryGetOccupantTeam(this ChessBoardSnapshot board, ChessSquare square, out ChessTeam team)
        {
            var piece = board.GetPiece(square);
            if (piece is null)
            {
                team = ChessTeam.None;
                return false;
            }
            
            team = piece.Team;
            return true;
        }


        internal static bool IsSquareValid(this ChessBoardSnapshot boardSnapshot, ChessSquare square)
        {
            return IsSquareValid(boardSnapshot.Board, square);
        }
        internal static bool IsSquareValid(this ChessBoard board, ChessSquare square)
        {
            return board.GetSquare(square.X, square.Y) is not null;
        }
    }
}