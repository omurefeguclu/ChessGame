using Omedya.ChessLib.Core;
using Omedya.Scripts.Core.Chess.Models;
using UnityEngine;

namespace Omedya.Scripts.Core.Chess.Modules
{
    public class LocalChessGameplayModule : BaseChessGameplayModule
    {
        public override void InitializeGame()
        {
            InitializeLocalPlayers();
            
            var game = ChessGame.CreateDefaultGame();
            
            Owner.StartGame(game);
        }

        private void InitializeLocalPlayers()
        {
            // Pick the local player team randomly
            var localPlayerTeam = Random.Range(0, 2) == 0 ? ChessTeam.White : ChessTeam.Black;
            
            // Create the local player
            var localPlayer = new ChessGamePlayer(localPlayerTeam, ChessPlayerType.Local);
            
            // Create the other local player
            var otherPlayer =
                new ChessGamePlayer(localPlayerTeam == ChessTeam.White ? ChessTeam.Black : ChessTeam.White,
                    ChessPlayerType.Local);
            
            // Add the players to the list
            Players.Add(localPlayer);
            Players.Add(otherPlayer);

            
        }
    }
}