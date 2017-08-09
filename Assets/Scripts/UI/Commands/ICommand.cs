using System;

namespace BattleCruisers.UI.Commands
{
    public interface ICommand
    {
        bool CanExecute { get; }

        event EventHandler CanExecuteChanged;

        void Execute();
        void EmitCanExecuteChanged();
    }
}