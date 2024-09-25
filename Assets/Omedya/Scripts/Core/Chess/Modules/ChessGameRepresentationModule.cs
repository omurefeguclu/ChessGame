using System.Collections;
using Omedya.ChessLib.Core;
using Omedya.Scripts.Core.Chess.Abstractions;
using Omedya.Scripts.Core.Chess.Components;
using UnityEngine;

namespace Omedya.Scripts.Core.Chess.Modules
{
    [System.Serializable]
    public class ChessGameRepresentationModule : IChessGameModule
    {
        private ChessPiece _highlightedPiece;
        private ChessSquare _highlightedSquare;
        
        private ChessGameManager _owner;
        private BaseChessGameplayModule _gameplayModule;
        
        [Header("References: ")]
        [SerializeField] private ChessBoardUI boardUI;
        
        public void Initialize(ChessGameManager owner)
        {
            _owner = owner;
            _gameplayModule = _owner.GetModule<LocalChessGameplayModule>();
            
            _owner.OnNewGame += OnNewGame;
            _owner.OnSnapshotChanged += OnSnapshotChangedEventReceived;
        }
        
        
        
        private void OnNewGame()
        {
            // Destroy previous game items
            boardUI.ClearBoard();
            
            // Create new game items
            boardUI.CreateBoard(_owner.CurrentGame.Board);
            
            
            // Wait two frames to let the UI stabilize
            // One frame is not working because the coroutine is running on another object
            _owner.StartCoroutine(NewGameUpdateState());
        }

        private IEnumerator NewGameUpdateState()
        {
            yield return null;
            yield return null;
            
            OnSnapshotChangedEventReceived();
        }
        public void OnSnapshotChangedEventReceived()
        {
            // Update the state of the game
            boardUI.UpdateBoardSnapshot(_owner.CurrentGame.CurrentSnapshot);
            
            HighlightItems(null, null);
        }


        private void HighlightItems(ChessSquare square, ChessPiece piece)
        {
            _highlightedPiece = piece;
            _highlightedSquare = square;
            
            // Update UI hightlights
            boardUI.UpdateHighlightedItems(_highlightedSquare, _highlightedPiece);
        }
        // Just UI movement for now
        public void OnSquareClicked(ChessSquare square)
        {
            if (!_gameplayModule.IsLocalPlayerTurn)
                return;

            var piece = _owner.CurrentGame.CurrentSnapshot.GetPiece(square);

            if (_highlightedPiece is not null)
            {
                var movement = _owner.CurrentGame.CurrentSnapshot.GetPossibleMovement(_highlightedSquare, square);
                
                if (movement is not null)
                {
                    // TODO: Check if movement is promotion, if so, show promotion UI
                    // Move the piece
                    _gameplayModule.LocalMovePiece(movement);
                    return;
                }

                if (piece is not null && piece.Team == _owner.CurrentGame.CurrentSnapshot.CurrentTurn)
                {
                    // Highlight the piece
                    HighlightItems(square, piece);
                }
                else
                {
                    // Unhighlight the piece
                    HighlightItems(null, null);
                }
            }
            else if (piece is not null && piece.Team == _owner.CurrentGame.CurrentSnapshot.CurrentTurn)
            {
                // Highlight the piece
                HighlightItems(square, piece);
            }
        }
        
        
    }
}