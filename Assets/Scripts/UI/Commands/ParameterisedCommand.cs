using System;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Commands
{
    public class ParameterisedCommand<T> : IParameterisedCommand<T>
	{
		private readonly Action<T> _action;
		private readonly Func<bool> _canExecute;

		public bool CanExecute
		{
			get { return _canExecute.Invoke(); }
		}

		public event EventHandler CanExecuteChanged;

		public ParameterisedCommand(Action<T> action, Func<bool> canExecute)
		{
			Helper.AssertIsNotNull(action, canExecute);

			_action = action;
			_canExecute = canExecute;
		}

		public void Execute(T parameter)
		{
			Assert.IsTrue(CanExecute);
            _action.Invoke(parameter);
		}

		public void EmitCanExecuteChanged()
		{
			if (CanExecuteChanged != null)
			{
				CanExecuteChanged.Invoke(this, EventArgs.Empty);
			}
		}
	}
}
