using System.Collections.Generic;
using Omedya.ChessLib.Core;

namespace Omedya.ChessLib.Pieces
{
    public class ChessKing : ChessPiece
    {
        public ChessKing(ChessTeam team) : base(team)
        {
        }

        public override IEnumerable<ChessMovement> GetPossibleMovements(ChessSquare position, ChessGame game, ChessBoard board)
        {
            throw new System.NotImplementedException();
        }
    }
}