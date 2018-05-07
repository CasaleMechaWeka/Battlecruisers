using System;
using BattleCruisers.Buildables.Buildings;

namespace BattleCruisers.UI.BattleScene.Buttons.ActivenessDeciders
{
    public class BuildingCategoryTutorialDecider : IFilter<BuildingCategory>, IBuildingCategoryPermitter
    {
        private BuildingCategory? _permittedCategory;
        public BuildingCategory? PermittedCategory
        {
            set
            {
                _permittedCategory = value;

                if (PotentialMatchChange != null)
                {
                    PotentialMatchChange.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler PotentialMatchChange;

        public BuildingCategoryTutorialDecider()
        {
            _permittedCategory = null;
        }

        public bool IsMatch(BuildingCategory category)
        {
            return _permittedCategory == category;
        }
    }
}
