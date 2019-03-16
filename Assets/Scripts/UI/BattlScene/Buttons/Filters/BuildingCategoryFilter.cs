using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.Filters;
using System;

namespace BattleCruisers.UI.BattleScene.Buttons.Filters
{
    public class BuildingCategoryFilter : IBroadcastingFilter<BuildingCategory>, IBuildingCategoryPermitter
    {
        private BuildingCategory? _permittedCategory;
        public BuildingCategory? PermittedCategory
        {
            set
            {
                _permittedCategory = value;

                PotentialMatchChange?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler PotentialMatchChange;

        public BuildingCategoryFilter()
        {
            _permittedCategory = null;
        }

        public bool IsMatch(BuildingCategory category)
        {
            return _permittedCategory == category;
        }
    }
}
