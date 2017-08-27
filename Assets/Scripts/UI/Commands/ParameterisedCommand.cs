using System;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Commands
{
    public class ParameterisedCommand<T> : CommandBase, IParameterisedCommand<T>
	{
		private readonly Action<T> _action;

		public ParameterisedCommand(Action<T> action, Func<bool> canExecute)
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
