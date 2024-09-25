using System;

namespace Omedya.ChessLib.Util
{
    public class RollbackUtil
    {
        private Action _rollbackAction;
        
        public RollbackUtil(Action rollbackAction)
        {
            _rollbackAction = rollbackAction;
        }
        
        public void Rollback()
        {
            _rollbackAction?.Invoke();
            
            _rollbackAction = null;
        }
    }
}