using System;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Commands
{
    public class PvPCommand : PvPCommandBase, IPvPCommand
    {
        private readonly Action _action;

        public PvPCommand(Action action, Func<bool> canExecute)
            : base(canExecute)
        {
            Helper.AssertIsNotNull(action, canExecute);
            _action = action;
        }

        public void Execute()
        {
            Assert.IsTrue(CanExecute);
            _action.Invoke();
        }

        public void ExecuteIfPossible()
        {
            if (CanExecute)
            {
                _action.Invoke();
            }
        }
    }
}
