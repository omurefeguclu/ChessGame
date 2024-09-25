using System;
using System.Collections.Generic;
using System.Linq;
using Omedya.ChessLib.Core;
using Omedya.ChessLib.SpecialMovements;
using Omedya.Scripts.Core.Chess.Abstractions;
using Omedya.Scripts.Core.Chess.Models;
using UnityEngine;
using UnityEngine.Serialization;

namespace Omedya.Scripts.Core.Chess.Components
{
    public class ChessBoardUI : MonoBehaviour, IChessBoardObject
    {
        [Header("Reference: ")] [SerializeField]
        private Transform squaresContainer;
        
        [Header("Prefabs: ")]
        [SerializeField] private ChessSquareUI squareUIPrefab;
        [SerializeField] private ChessPieceUI piecePrefab;
        
        // Data fields
        private ChessBoard _chessBoard;
        private ChessBoardSnapshot _currentSnapshot;
        
        private readonly Dictionary<ChessSquare, ChessSquareUI> _squareUIs = new Dictionary<ChessSquare, ChessSquareUI>();
        private readonly Dictionary<ChessPiece, ChessPieceUI> _pieceUIs = new Dictionary<ChessPiece, ChessPieceUI>();
        
        public void CreateBoard(ChessBoard board)
        {
            _chessBoard = board;
            
            CreateSquares();
        }

        public void UpdateBoardSnapshot(ChessBoardSnapshot snapshot)
        {
            _currentSnapshot = snapshot;
            
            UpdateBoardSnapshot();
        }

        public void UpdateHighlightedItems(ChessSquare square, ChessPiece piece)
        {
            var movableSquares = _currentSnapshot.GetPossibleMovementsByStart(square)
                .Select(x => x.End).ToList();
            
            foreach (var squareUI in _squareUIs.Values)
            {
                if(movableSquares.Contains(squareUI.Square))
                    squareUI.UpdateHighlightState(ChessSquareHighlightState.HighlightedMove);
                else if(square == squareUI.Square)
                    squareUI.UpdateHighlightState(ChessSquareHighlightState.HighlightedPiece);
                else
                    squareUI.UpdateHighlightState(ChessSquareHighlightState.None);
            }
            
        }
        
        #region Setup

        private void CreateSquares()
        {
            for (var y = 1; y <= _chessBoard.Height; y++)
            {
                for (var x = 1; x <= _chessBoard.Width; x++)
                {
                    var square = _chessBoard.GetSquare(x, y);
                    if (square is null)
                    {
                        // Add a null square to the layout to handle the board shape (if needed)
                        
                        continue;
                    }
                    
                    var squareUI = Instantiate(squareUIPrefab, squaresContainer);
                    
                    _squareUIs[square] = squareUI;
                    squareUI.InitializeSquare(square);
                }
            }
        }
        
        #endregion
        
        #region Pieces (might move to another class)
        
        private void UpdateBoardSnapshot()
        {
            var lastMovement = _currentSnapshot.LastMovement;
            // To animate the last movement, we build the snapshot before the last movement
            var firstSnapshot = lastMovement is not null ? _currentSnapshot.PreviousSnapshot : _currentSnapshot;

            // Copy the pieces on the screen dictionary to handle updating the pieces
            var piecesToUpdate = new Dictionary<ChessPiece, ChessPieceUI>(_pieceUIs);

            // Update the positions of the pieces
            foreach (var square in _squareUIs.Keys)
            {
                var piece = firstSnapshot.GetPiece(square);
                if (piece is not null)
                {
                    // Remove the piece from the dictionary (if exists) to handle the destruction of the pieces that are not in the current snapshot
                    piecesToUpdate.Remove(piece);
                    
                    UpdatePiecePosition(piece, square);
                }
            }
            
            // Destroy the pieces that are not in the current snapshot
            foreach (var piece in piecesToUpdate.Keys)
            {
                DestroyPieceImmediate(piece);
            }
            
            
            // Animate the last movement
            if (lastMovement is not null)
            {
                // TODO: Implement DOTween animation, for now just update the position
                UpdatePiecePosition(lastMovement.MovedPiece, lastMovement.Movement.End, animate: true);
                
                if(lastMovement.CapturedPiece is not null)
                    PieceCaptured(lastMovement.CapturedPiece);
                
                // Issue: Castling movement is not represented and promotion is not animated
                if(lastMovement.Movement is CastlingMove castlingMove)
                {
                    var rook = firstSnapshot.GetPiece(castlingMove.RookStartSquare);
                    
                    UpdatePiecePosition(rook, castlingMove.RookEndSquare, animate: true);
                }
                
            }
        }

        private void UpdatePiecePosition(ChessPiece piece, ChessSquare to, bool animate = false)
        {
            if (!_pieceUIs.TryGetValue(piece, out var pieceUI))
            {
                // TODO: Instantiate into the canvas
                
                pieceUI = Instantiate(piecePrefab, transform);
                
                _pieceUIs[piece] = pieceUI;
            }
            
            if(!_squareUIs.TryGetValue(to, out var squareUI))
                throw new System.Exception("Square not found in the dictionary");
                
            // TODO: Animate the piece to the new position, for now just update the position
            pieceUI.transform.localPosition = squareUI.PiecePosition;
        }
        private void PieceCaptured(ChessPiece piece)
        {
            if (!_pieceUIs.TryGetValue(piece, out var pieceUI))
            {
                Debug.LogError("Piece not found in the dictionary");
                return;
            }
            
            // TODO: Animate dissolving the piece, for now just destroy
            Destroy(pieceUI.gameObject);
            _pieceUIs.Remove(piece);
        }
        private void DestroyPieceImmediate(ChessPiece piece)
        {
            if (!_pieceUIs.TryGetValue(piece, out var pieceUI))
            {
                Debug.LogError("Piece not found in the dictionary");
                return;
            }
            
            Destroy(pieceUI.gameObject);
            _pieceUIs.Remove(piece);
        }
        
        #endregion
        
        
        #region Cleanup

        public void ClearBoard()
        {
            foreach (var pieceUI in _pieceUIs.Values)
            {
                Destroy(pieceUI.gameObject);
            }
            
            _pieceUIs.Clear();
            
            foreach (var squareUI in _squareUIs.Values)
            {
                Destroy(squareUI.gameObject);
            }
            
            _squareUIs.Clear();
            
            _chessBoard = null;
            _currentSnapshot = null;
        }

        #endregion
    }
}