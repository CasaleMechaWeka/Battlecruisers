using System.Collections.Generic;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.LoadoutItems
{
    public class LoadoutUnitItemsRow : LoadoutBuildableItemsRow<IUnit, UnitKey>
    {
        private UnitCategory _unitCategory;

        protected override int NumOfLockedBuildables { get { return _lockedInfo.NumOfLockedUnits(_unitCategory); } }

        public void Initialise(IItemsRowArgs<IUnit> args, UnitCategory unitCategory)
        {
            base.Initialise(args);

            _unitCategory = unitCategory;
        }

        protected override LoadoutItem<IUnit> CreateItem(IUnit item)
        {
            return _uiFactory.CreateLoadoutUnitItem(_layoutGroup, item);
        }

        protected override IUnit GetBuildablePrefab(UnitKey prefabKey)
        {
            return _prefabFactory.GetUnitWrapperPrefab(prefabKey).Buildable;
        }

        protected override IList<IUnit> GetLoadoutBuildablePrefabs()
        {
            return GetBuildablePrefabs(_gameModel.PlayerLoadout.GetUnits(_unitCategory));
        }
    }
}
