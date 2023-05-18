using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Filters;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters
{
    public class PvPBuildingCategoryFilter : IPvPBroadcastingFilter<PvPBuildingCategory>, IPvPBuildingCategoryPermitter
    {
        private bool _allowAll;
        private PvPBuildingCategory? _permittedCategory;

        public event EventHandler PotentialMatchChange;

        public PvPBuildingCategoryFilter()
        {
            _allowAll = false;
            _permittedCategory = null;
        }

        public bool IsMatch(PvPBuildingCategory category)
        {
            return
                _allowAll
                || _permittedCategory == category;
        }

        public void AllowSingleCategory(PvPBuildingCategory buildingCategory)
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
