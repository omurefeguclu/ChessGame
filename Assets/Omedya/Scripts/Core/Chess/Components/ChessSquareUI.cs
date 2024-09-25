using System;
using Omedya.ChessLib.Core;
using Omedya.Scripts.Core.Chess.Abstractions;
using Omedya.Scripts.Core.Chess.Models;
using Omedya.Scripts.Core.Chess.Modules;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Omedya.Scripts.Core.Chess.Components
{
    [RequireComponent(typeof(Image))]
    public class ChessSquareUI : MonoBehaviour, IChessSquareObject, IPointerClickHandler
    {
        // Get the center of the rect
        public Vector3 PiecePosition => transform.localPosition;
        public ChessSquare Square => _square;
        
        [Header("For test: ")]
        [SerializeField] private Color lightColor;
        [SerializeField] private Color darkColor;
        
        
        // Data fields
        private ChessSquare _square;
        private ChessSquareHighlightState _highlightState;
        
        // Component fields
        private Image _renderer;
        
        public void InitializeSquare(ChessSquare square)
        {
            _renderer = GetComponent<Image>();
            _square = square;
            _highlightState = ChessSquareHighlightState.None;
            
            // Update the color of the square
            UpdateSquareColor();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            // Notify the game manager
            ChessGameManager.Instance.GetModule<ChessGameRepresentationModule>().OnSquareClicked(_square);
        }
        
        private void UpdateSquareColor()
        {
            switch (_highlightState)
            {
                case ChessSquareHighlightState.None:
                    _renderer.color = _square.X % 2 == _square.Y % 2 ? lightColor : darkColor;
                    break;
                case ChessSquareHighlightState.HighlightedMove:
                    _renderer.color = Color.green;
                    break;
                case ChessSquareHighlightState.HighlightedPiece:
                    _renderer.color = Color.red;
                    break;
                default:
                    Debug.Log("Square highlight state not implemented: " + _highlightState);
                    break;
            }
        }
        public void UpdateHighlightState(ChessSquareHighlightState state)
        {
            _highlightState = state;
            
            UpdateSquareColor();
        }

        
    }
}