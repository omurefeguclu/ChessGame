using System;
using System.Collections.Generic;
using Omedya.ChessLib.Core;
using Omedya.Scripts.Core.Chess.Abstractions;
using Omedya.Scripts.Core.Chess.Models;
using UnityEngine;

namespace Omedya.Scripts.Core.Chess.Modules
{
    public abstract class BaseChessGameplayModule : IChessGameModule
    {
        public List<ChessGamePlayer> Players { get; } = new List<ChessGamePlayer>();

        public ChessGamePlayer CurrentTurnPlayer =>
            Players.Find(x => x.Team == Owner.CurrentGame.CurrentSnapshot.CurrentTurn);
        public bool IsLocalPlayerTurn => CurrentTurnPlayer.PlayerType == ChessPlayerType.Local;
        
        protected ChessGameManager Owner;
        
        public void Initialize(ChessGameManager owner)
        {
            Owner = owner;
        }

        public abstract void InitializeGame();
        
        public virtual void LocalMovePiece(ChessMovement movement)
        {
            // Move the piece
            if (Owner.CurrentGame.MovePiece(movement))
            {
                // Move is valid
                Debug.Log(Owner.CurrentGame.CurrentSnapshot.LastMovement.ToString());
                
                // Update the state
                Owner.OnSnapshotReceived();
            }
            else
            {
                // Invalid move
                Debug.LogError("Invalid move");
            }
            
        }
        
    }
}