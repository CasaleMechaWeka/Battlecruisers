using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.LoadoutItems;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.UnlockedItems;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows
{
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

        protected abstract IList<TItem> GetUnlockedBuildablePrefabs();

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
            // All buildable items are now in the loadout, hence no more selection ligic :/
            return false;
		}
	}
}
