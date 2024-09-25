using System;
using Omedya.ChessLib.Core;
using Omedya.Scripts.Core.Chess.Abstractions;
using Omedya.Scripts.Core.Chess.Modules;
using Omedya.Scripts.Core.Shared.ModuleSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace Omedya.Scripts.Core.Chess
{
    public class ChessGameManager : MonoBehaviour
    {
        // Singleton
        public static ChessGameManager Instance { get; private set; }
        
        [SerializeField] private bool isOnline; 
        
        // Modules
        private BaseChessGameplayModule _gameplayModule;
        [SerializeField] private ChessGameRepresentationModule representationModule;
        
        public ChessGame CurrentGame { get; private set; }
        public event Action OnNewGame;
        public event Action OnSnapshotChanged;

        private void Awake()
        {
            Instance = this;
            
            
            // Implement game modules
            // if online, use online module
            _gameplayModule = new LocalChessGameplayModule();
            
            // Add modules to the store
            ImplementModule(_gameplayModule);
            ImplementModule(representationModule);
            
            
            _gameplayModule.InitializeGame();
        }

        // Game logic
        public void StartGame(ChessGame game)
        {
            CurrentGame = game;
            
            // Start the game
            OnNewGame?.Invoke();
            
        }

        public void OnSnapshotReceived()
        {
            OnSnapshotChanged?.Invoke();
        }

        
        
        #region Game Modules

        private readonly ModuleStore<IChessGameModule> _moduleStore = new ModuleStore<IChessGameModule>();
        
        protected TModule ImplementModule<TModule>(TModule module) where TModule : IChessGameModule
        {
            _moduleStore.AddModule(module);

            module.Initialize(this);
            return module;
        }

        public TModule GetModule<TModule>() where TModule : IChessGameModule
        {
            return _moduleStore.GetModule<TModule>();
        }
    
        #endregion
    }
}