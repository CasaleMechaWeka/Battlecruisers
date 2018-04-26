using System;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public interface IActivenessDecider<TElement>
    {
        /// <summary>
        /// Emitted when button activeness may have changed (eg, due to the 
        /// drone manager's number of drones changing).
        /// </summary>
        event EventHandler PotentialActivenessChange;

        bool ShouldBeEnabled(TElement element);
    }
}
