using Omedya.ChessLib.Core;
using Omedya.ChessLib.Pieces;

namespace Omedya.ChessLib.SpecialMovements
{
    public class PromotionMove : ChessSpecialMovement
    {
        private ChessPiece _promotionNewPiece;
        private ChessPiece _promotedPawn;
        
        private ChessPiece _capturedPiece;
        
        
        public PromotionMove(ChessSquare start, ChessSquare end) : base(start, end)
        {
        }

        public void SelectPromotionPiece(ChessPiece piece)
        {
            if(piece is ChessKing)
                throw new System.Exception("King cannot be selected as promotion piece");
            
            _promotionNewPiece = piece;
        }


        public override void Execute(ChessBoardSnapshot boardSnapshot)
        {
            _promotedPawn = boardSnapshot.GetPiece(Start);
            _capturedPiece = boardSnapshot.GetPiece(End);
            
            // If promotion piece is not selected, default to Queen (for possible movement calc.)
            _promotionNewPiece ??= new ChessQueen(_promotedPawn.Team);
            
            boardSnapshot.SetPiece(Start, null);
            boardSnapshot.SetPiece(End, _promotionNewPiece);
        }

        public override void Rollback(ChessBoardSnapshot boardSnapshot)
        {
            boardSnapshot.SetPiece(Start, _promotedPawn);
            boardSnapshot.SetPiece(End, _capturedPiece);
        }
    }
}