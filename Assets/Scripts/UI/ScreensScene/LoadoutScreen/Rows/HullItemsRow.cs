using System.Collections.Generic;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Fetchers;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.LoadoutItems;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.UnlockedItems;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows
{
    public class HullItemsRow : ItemsRow<ICruiser>
	{
		private readonly LoadoutHullItem _loadoutHull;
        private readonly UnlockedHullItemsRow _unlockedHullsRow;
		private readonly IDictionary<ICruiser, HullKey> _hullToKey;

		public HullItemsRow(
            IGameModel gameModel, 
            IPrefabFactory prefabFactory, 
            IUIFactory uiFactory, 
            LoadoutHullItem loadoutHull, 
            UnlockedHullItemsRow unlockedHullsRow, 
            IItemDetailsManager<ICruiser> cruiserDetailsManager)
            : base(gameModel, prefabFactory, uiFactory, cruiserDetailsManager)
		{
            Helper.AssertIsNotNull(loadoutHull, unlockedHullsRow);

			_loadoutHull = loadoutHull;
            _unlockedHullsRow = unlockedHullsRow;

			_hullToKey = new Dictionary<ICruiser, HullKey>();
        }

        public override void SetupUI()
        {
			Cruiser loadoutCruiser = _prefabFactory.GetCruiserPrefab(_gameModel.PlayerLoadout.Hull);
            _loadoutHull.Initialise(loadoutCruiser, _detailsManager);
			
            _unlockedHullsRow.Initialise(this, _uiFactory, GetUnlockedHullPrefabs(), loadoutCruiser, _detailsManager);
            _unlockedHullsRow.SetupUI();
        }

		private IList<ICruiser> GetUnlockedHullPrefabs()
		{
			IList<HullKey> hullKeys = _gameModel.UnlockedHulls;
			IList<ICruiser> prefabs = new List<ICruiser>();

			foreach (HullKey hullKey in hullKeys)
			{
				ICruiser hull = _prefabFactory.GetCruiserPrefab(hullKey);
				prefabs.Add(hull);
				_hullToKey.Add(hull, hullKey);
			}

			return prefabs;
		}

		public override bool SelectUnlockedItem(UnlockedItem<ICruiser> hullItem)
		{
			ICruiser hull = hullItem.Item;
			_gameModel.PlayerLoadout.Hull = _hullToKey[hull];
			
			// Update UI
			_loadoutHull.UpdateHull(hull);
			_unlockedHullsRow.UpdateSelectedHull(hull);

			return true;
		}
	}
}
