using System;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.Commands
{
    public class Command : ICommand
	{
        private readonly Action _action;
        private readonly Func<bool> _canExecute;

		public bool CanExecute 
        { 
            get { return _canExecute.Invoke(); }
        }

		public event EventHandler CanExecuteChanged;

        public Command(Action action, Func<bool> canExecute)
        {
            Helper.AssertIsNotNull(action, canExecute);

            _action = action;
            _canExecute = canExecute;
        }

		public void Execute()
        {
            _action.Invoke();
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
