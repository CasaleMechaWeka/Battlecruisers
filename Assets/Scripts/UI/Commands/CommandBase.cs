using System;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Commands
{
    public abstract class CommandBase : ICommandBase
	{
		private readonly Func<bool> _canExecute;

		public bool CanExecute
		{
			get { return _canExecute.Invoke(); }
		}

		public event EventHandler CanExecuteChanged;

		public CommandBase(Func<bool> canExecute)
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
