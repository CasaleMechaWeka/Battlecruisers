using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.Filters;
using System;

namespace BattleCruisers.UI.BattleScene.Buttons.Filters
{
    // FELIX  Update tests
    public class BuildingCategoryFilter : IBroadcastingFilter<BuildingCategory>, IBuildingCategoryPermitter
    {
        private bool _allowAll;
        private BuildingCategory? _permittedCategory;

        public event EventHandler PotentialMatchChange;

        public BuildingCategoryFilter()
        {
            _allowAll = false;
            _permittedCategory = null;
        }

        public bool IsMatch(BuildingCategory category)
        {
            return
                _allowAll
                || _permittedCategory == category;
        }

        public void AllowSingleCategory(BuildingCategory buildingCategory)
        {
            _allowAll = false;
            _permittedCategory = buildingCategory;

            PotentialMatchChange?.Invoke(this, EventArgs.Empty);
        }

        public void AllowAllCategories()
        {
            _allowAll = true;
            _permittedCategory = null;
            PotentialMatchChange?.Invoke(this, EventArgs.Empty);
        }

        public void AllowNoCategories()
        {
            _allowAll = false;
            _permittedCategory = null;
            PotentialMatchChange?.Invoke(this, EventArgs.Empty);
        }
    }
}
