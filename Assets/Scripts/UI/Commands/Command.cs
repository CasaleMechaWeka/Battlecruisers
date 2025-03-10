using System;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Commands
{
    public class Command :  CommandBase, ICommand
	{
        private readonly Action _action;

        public Command(Action action, Func<bool> canExecute)
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
