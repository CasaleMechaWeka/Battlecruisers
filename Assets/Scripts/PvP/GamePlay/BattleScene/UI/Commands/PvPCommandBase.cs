using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Commands
{
    public abstract class PvPCommandBase : IPvPCommandBase
    {
        private readonly Func<bool> _canExecute;

        public bool CanExecute
        {
            get { return _canExecute.Invoke(); }
        }

        public event EventHandler CanExecuteChanged;

        public PvPCommandBase(Func<bool> canExecute)
        {
            Assert.IsNotNull(canExecute);
            _canExecute = canExecute;
        }

        public void EmitCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}

