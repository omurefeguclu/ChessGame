using Omedya.ChessLib.Core;

namespace Omedya.Scripts.Core.Chess.Models
{
    public class ChessGamePlayer
    {
        // constructor
        public ChessGamePlayer(ChessTeam team, ChessPlayerType playerType)
        {
            Team = team;
            PlayerType = playerType;
        }
        
        public ChessTeam Team { get; }
        public ChessPlayerType PlayerType { get; }
    }
}