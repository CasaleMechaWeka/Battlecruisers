using System.Collections.Generic;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.LoadoutItems;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows
{
    public class UnitItemsRow : BuildableItemsRow<IUnit, UnitKey>
    {
        private readonly UnitCategory _unitCategory;

        protected override int NumOfLockedBuildables { get { return _lockedInfo.NumOfLockedUnits(_unitCategory); } }

        public UnitItemsRow(
            IItemsRowArgs<IUnit> args,
            LoadoutBuildableItemsRow<IUnit> loadoutRow, 
            UnitCategory unitCategory) 
            : base(args, loadoutRow)
        {
            _unitCategory = unitCategory;
        }

        protected override IList<IUnit> GetLoadoutBuildablePrefabs()
        {
            return GetBuildablePrefabs(_gameModel.PlayerLoadout.GetUnits(_unitCategory), addToDictionary: false);
        }

        protected override IList<IUnit> GetUnlockedBuildablePrefabs()
        {
            return GetBuildablePrefabs(_gameModel.GetUnlockedUnits(_unitCategory), addToDictionary: true);
        }

        protected override IUnit GetBuildablePrefab(UnitKey prefabKey)
		{
            return _prefabFactory.GetUnitWrapperPrefab(prefabKey).Buildable;
		}
    }
}
