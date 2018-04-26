using System;
using BattleCruisers.Buildables.Buildings;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public class BuildingCategoryTutorialDecider : IBuildableButtonActivenessDecider<BuildingCategory>, IBuildingCategoryPermitter
    {
        private BuildingCategory _permittedCategory;
        public BuildingCategory PermittedCategory
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

        public bool ShouldBeEnabled(BuildingCategory category)
        {
            return _permittedCategory == category;
        }
    }
}
