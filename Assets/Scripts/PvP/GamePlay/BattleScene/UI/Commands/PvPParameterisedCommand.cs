using System;
using UnityEngine.Assertions;


namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Commands
{
    public class PvPParameterisedCommand<T> : PvPCommandBase, IPvPParameterisedCommand<T>
    {
        private readonly Action<T> _action;

        public PvPParameterisedCommand(Action<T> action, Func<bool> canExecute)
            : base(canExecute)
        {
            Assert.IsNotNull(action);
            _action = action;
        }

        public void Execute(T parameter)
        {
            Assert.IsTrue(CanExecute);
            _action.Invoke(parameter);
        }
    }
}
