using System.Collections.Generic;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Fetchers;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.LoadoutItems;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.UnlockedItems;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public class UnitItemsRow : BuildableItemsRow<IUnit, UnitKey>
    {
        private readonly UnitCategory _unitCategory;

        public UnitItemsRow(
            IGameModel gameModel, 
            IPrefabFactory prefabFactory, 
            IUIFactory uiFactory, 
            LoadoutBuildableItemsRow<IUnit> loadoutRow, 
            UnlockedBuildableItemsRow<IUnit> unlockedRow, 
            IItemDetailsManager<IUnit> detailsManager,
            UnitCategory unitCategory) 
            : base(gameModel, prefabFactory, uiFactory, loadoutRow, unlockedRow, detailsManager)
        {
            _unitCategory = unitCategory;
        }

        protected override IList<IUnit> GetLoadoutBuildablePrefabs()
        {
            return GetBuildablePrefabs(_gameModel.PlayerLoadout.GetUnits(_unitCategory), addToDictionary: false);
        }

        protected override IList<IUnit> GetUnlockedBuildingPrefabs()
        {
            return GetBuildablePrefabs(_gameModel.GetUnlockedUnits(_unitCategory), addToDictionary: true);
        }

        protected override IUnit GetBuildablePrefab(UnitKey prefabKey)
		{
            return _prefabFactory.GetUnitWrapperPrefab(prefabKey).Buildable;
		}
		
        protected override void AddToLoadoutModel(UnitKey buildableKey)
        {
            _gameModel.PlayerLoadout.AddUnit(buildableKey);
        }

        protected override void RemoveFromLoadoutModel(UnitKey buildableKey)
        {
            _gameModel.PlayerLoadout.RemoveUnit(buildableKey);
        }
    }
}
