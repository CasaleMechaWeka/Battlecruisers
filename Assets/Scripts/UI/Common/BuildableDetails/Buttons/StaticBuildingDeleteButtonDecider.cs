using System;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.BattleScene.Buttons;

namespace BattleCruisers.UI.Common.BuildableDetails.Buttons
{
    // FELIX  Avoid duplicate code with other static decider :)
    public class StaticBuildingDeleteButtonDecider: IActivenessDecider<IBuilding>
    {
        private readonly bool _shouldBeEnabled;

#pragma warning disable 67  // Unused event
        public event EventHandler PotentialActivenessChange;
#pragma warning restore 67  // Unused event

        public StaticBuildingDeleteButtonDecider(bool shouldBeEnabled)
        {
            _shouldBeEnabled = shouldBeEnabled;
        }

        public bool ShouldBeEnabled(IBuilding building)
        {
            return _shouldBeEnabled;
        }
    }
}
