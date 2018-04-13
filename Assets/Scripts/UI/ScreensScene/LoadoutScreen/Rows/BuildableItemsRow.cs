using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.LoadoutItems;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.UnlockedItems;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows
{
    /// <summary>
    /// // FELIX  Remove obsolete methods :(
    /// Have methods for adding/removing buildables from loadout, even though this
    /// functionality has been removed from the UI.  Leave the methods in case
    /// this functionality is ever resurrected :)
    /// </summary>
    public abstract class BuildableItemsRow<TItem, TPrefabKey> : ItemsRow<TItem> 
        where TItem : IBuildable 
        where TPrefabKey : IPrefabKey
	{
        private readonly LoadoutBuildableItemsRow<TItem> _loadoutRow;
        private readonly IDictionary<TItem, TPrefabKey> _buildableToKey;

        protected abstract int NumOfLockedBuildables { get; }

        protected BuildableItemsRow(IItemsRowArgs<TItem> args, LoadoutBuildableItemsRow<TItem> loadoutRow)
            : base(args)
		{
			_loadoutRow = loadoutRow;
            _buildableToKey = new Dictionary<TItem, TPrefabKey>();
        }

        public override void SetupUI()
        {
            IList<TItem> loadoutBuildables = GetLoadoutBuildablePrefabs();
            _loadoutRow.Initialise(_uiFactory, loadoutBuildables, _detailsManager, NumOfLockedBuildables);
            _loadoutRow.SetupUI();
        }

        protected abstract IList<TItem> GetLoadoutBuildablePrefabs();

        // FELIX  Rename to Buildable instead of Building
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
