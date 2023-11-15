using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Commands
{
    public interface IPvPCommandBase
    {
        bool CanExecute { get; }

        event EventHandler CanExecuteChanged;

        void EmitCanExecuteChanged();
    }
}