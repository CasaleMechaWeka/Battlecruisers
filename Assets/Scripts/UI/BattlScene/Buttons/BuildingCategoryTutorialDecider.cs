using System;
using BattleCruisers.Buildables.Buildings;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    // FELIX  Test :D
    public class BuildingCategoryTutorialDecider : IBuildableButtonActivenessDecider<BuildingCategory>, IBuildingCategoryPermitter
    {
        private BuildingCategory? _permittedCategory;
        public BuildingCategory? PermittedCategory
        {
            set
            {
                _permittedCategory = value;

                if (PotentialActivenessChange != null)
                {
                    PotentialActivenessChange.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler PotentialActivenessChange;

        public BuildingCategoryTutorialDecider()
        {
            _permittedCategory = null;
        }

        public bool ShouldBeEnabled(BuildingCategory category)
        {
            return _permittedCategory == category;
        }
    }
}
