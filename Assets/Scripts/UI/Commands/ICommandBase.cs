using System;

namespace BattleCruisers.UI.Commands
{
    public interface ICommandBase
    {
        bool CanExecute { get; }

        event EventHandler CanExecuteChanged;

        void EmitCanExecuteChanged();
    }
}
