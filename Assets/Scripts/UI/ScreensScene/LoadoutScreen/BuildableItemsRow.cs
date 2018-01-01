using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Fetchers;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.LoadoutItems;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.UnlockedItems;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public abstract class BuildableItemsRow<TItem, TPrefabKey> : ItemsRow<TItem> 
        where TItem : IBuildable 
        where TPrefabKey : IPrefabKey
	{
        private readonly LoadoutBuildableItemsRow<TItem> _loadoutRow;
        private readonly UnlockedBuildableItemsRow<TItem> _unlockedRow;

        // FELIX  Common with HullItemsRow...  Move to parent?
        private readonly IDictionary<TItem, TPrefabKey> _buildableToKey;

		protected BuildableItemsRow(
            IGameModel gameModel, 
            IPrefabFactory prefabFactory, 
            IUIFactory uiFactory, 
            LoadoutBuildableItemsRow<TItem> loadoutRow, 
            UnlockedBuildableItemsRow<TItem> unlockedRow, 
            IItemDetailsManager<TItem> detailsManager)
            : base(gameModel, prefabFactory, uiFactory, detailsManager)
		{
			_loadoutRow = loadoutRow;
			_unlockedRow = unlockedRow;

            _buildableToKey = new Dictionary<TItem, TPrefabKey>();
        }

        public override void SetupUI()
        {
            IList<TItem> loadoutBuildables = GetLoadoutBuildablePrefabs();
			_loadoutRow.Initialise(_uiFactory, loadoutBuildables, _detailsManager);
            // FELIX  Call SetupUI()

            IList<TItem> unlockedBuildables = GetUnlockedBuildingPrefabs();
			_unlockedRow.Initialise(this, _uiFactory, unlockedBuildables, loadoutBuildables, _detailsManager);
            _unlockedRow.SetupUI();
        }

        protected abstract IList<TItem> GetLoadoutBuildablePrefabs();

        protected abstract IList<TItem> GetUnlockedBuildingPrefabs();

        protected IList<TItem> GetBuildablePrefabs(IList<TPrefabKey> buildableKeys, bool addToDictionary)
		{
            IList<TItem> prefabs = new List<TItem>();

            foreach (TPrefabKey key in buildableKeys)
			{
                TItem buildable = GetBuildablePrefab(key);
				prefabs.Add(buildable);

				if (addToDictionary)
				{
                    _buildableToKey.Add(buildable, key);
				}
			}

			return prefabs;
		}

        protected abstract TItem GetBuildablePrefab(TPrefabKey prefabKey);

        public override bool SelectUnlockedItem(UnlockedItem<TItem> buildableItem)
		{
			bool isItemInLoadout = false;

			if (buildableItem.IsItemInLoadout)
			{
                RemoveBuildableFromLoadout(buildableItem.Item);
			}
			else if (CanAddBuilding())
			{
				AddBuildableToLoadout(buildableItem.Item);
				isItemInLoadout = true;
			}

			return isItemInLoadout;
		}

		private bool CanAddBuilding()
		{
			return _loadoutRow.CanAddBuildable();
		}

        private void AddBuildableToLoadout(TItem buildable)
        {
            _loadoutRow.AddBuildable(buildable);
            AddToLoadoutModel(_buildableToKey[buildable]);
        }

        protected abstract void AddToLoadoutModel(TPrefabKey buildableKey);

        private void RemoveBuildableFromLoadout(TItem buildable)
        {
            _loadoutRow.RemoveBuildable(buildable);
            RemoveFromLoadoutModel(_buildableToKey[buildable]);
        }

        protected abstract void RemoveFromLoadoutModel(TPrefabKey buildableKey);
	}
}
