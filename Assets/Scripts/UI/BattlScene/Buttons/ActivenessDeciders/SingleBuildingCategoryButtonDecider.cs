using System;
using BattleCruisers.Buildables.Buildings;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Buttons.ActivenessDeciders
{
    // FELIX  Delete
    public class SingleBuildingCategoryButtonDecider : IActivenessDecider
    {
        private readonly BuildingCategory _category;
        private readonly IActivenessDecider<BuildingCategory> _generalDecider;

        public bool ShouldBeEnabled { get { return _generalDecider.ShouldBeEnabled(_category); } }

        public event EventHandler PotentialActivenessChange
        {
            add { _generalDecider.PotentialActivenessChange += value; }
            remove { _generalDecider.PotentialActivenessChange -= value; }
        }

        public SingleBuildingCategoryButtonDecider(BuildingCategory category, IActivenessDecider<BuildingCategory> generalDecider)
        {
            Assert.IsNotNull(generalDecider);

            _category = category;
            _generalDecider = generalDecider;
        }
    }
}
