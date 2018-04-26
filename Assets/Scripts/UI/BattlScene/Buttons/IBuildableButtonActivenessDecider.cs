using System;
using BattleCruisers.Buildables;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public interface IBuildableButtonActivenessDecider
    {
        /// <summary>
        /// Emitted when button activeness may have changed (eg, due to the 
        /// drone manager's number of drones changing).
        /// </summary>
        event EventHandler PotentialActivenessChange;

        bool ShouldBeEnabled(IBuildable buildable);
    }
}
