using System;
using BattleCruisers.Buildables.Buildings;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    // FELIX  Test? :P
    public class BuildingCategoryStaticDecider : IBuildableButtonActivenessDecider<BuildingCategory>
    {
        private readonly bool _shouldBeEnabled;
		
        #pragma warning disable 67  // Unused event
        public event EventHandler PotentialActivenessChange;
        #pragma warning restore 67  // Unused event

        public BuildingCategoryStaticDecider(bool shouldBeEnabled)
        {
            _shouldBeEnabled = shouldBeEnabled;
        }

        public bool ShouldBeEnabled(BuildingCategory category)
        {
            return _shouldBeEnabled;
        }
    }
}
