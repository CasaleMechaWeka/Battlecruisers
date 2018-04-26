using System;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    // FELIX  Rename
    public interface IBuildableButtonActivenessDecider<TButton>
    {
        /// <summary>
        /// Emitted when button activeness may have changed (eg, due to the 
        /// drone manager's number of drones changing).
        /// </summary>
        event EventHandler PotentialActivenessChange;

        bool ShouldBeEnabled(TButton buildable);
    }
}
