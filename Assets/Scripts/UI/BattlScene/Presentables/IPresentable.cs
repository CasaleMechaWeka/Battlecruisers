using System;

namespace BattleCruisers.UI.BattleScene.Presentables
{
    public interface IPresentable
    {
        event EventHandler Presented;
        event EventHandler Dismissed;

        // About to be shown
        void OnPresenting(object activationParameter);

        // About to be hidden
        void OnDismissing();
    }
}
