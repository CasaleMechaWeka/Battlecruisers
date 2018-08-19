using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Utils.Fetchers;
using System.Collections.Generic;
using System.Linq;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.LoadoutItems
{
    public class LoadoutUnitItemsRow : LoadoutBuildableItemsRow<IUnit, UnitKey>
    {
        private UnitCategory _unitCategory;

        public void Initialise(IItemsRowArgs<IUnit> args, UnitCategory unitCategory)
        {
            // Will be used in overridden method called by base.Initialise() :/
            _unitCategory = unitCategory;

            base.Initialise(args);
        }

        protected override IList<UnitKey> FindBuildableKeys(IStaticData staticData)
        {
            return
                staticData.UnitKeys
                    .Where(key => key.UnitCategory == _unitCategory)
                    .ToList();
        }

        protected override IUnit GetBuildablePrefab(IPrefabFactory prefabFactory, UnitKey buildableKey)
        {
            return prefabFactory.GetUnitWrapperPrefab(buildableKey).Buildable;
        }
    }
}