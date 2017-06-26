using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Data.PrefabKeys;
using BattleCruisers.Fetchers;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.LoadoutItems;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.UnlockedItems;
using System;
using System.Collections.Generic;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
	public class HullItemsRow : ItemsRow<Cruiser>
	{
		private readonly LoadoutHullItem _loadoutHull;
		private readonly UnlockedHullItemsRow _unlockedHullsRow;
		private readonly IDictionary<Cruiser, HullKey> _hullToKey;

		public HullItemsRow(IGameModel gameModel, IPrefabFactory prefabFactory, IUIFactory uiFactory, LoadoutHullItem loadoutHull, 
			UnlockedHullItemsRow unlockedHullsRow, CruiserDetailsManager cruiserDetailsManager)
			: base(gameModel, prefabFactory)
		{
			_loadoutHull = loadoutHull;
			_unlockedHullsRow = unlockedHullsRow;

			_hullToKey = new Dictionary<Cruiser, HullKey>();

			Cruiser loadoutCruiser = _prefabFactory.GetCruiserPrefab(_gameModel.PlayerLoadout.Hull);
			_loadoutHull.Initialise(loadoutCruiser, cruiserDetailsManager);
			_unlockedHullsRow.Initialise(this, uiFactory, GetUnlockedHullPrefabs(), loadoutCruiser, cruiserDetailsManager);
		}

		private IList<Cruiser> GetUnlockedHullPrefabs()
		{
			IList<HullKey> hullKeys = _gameModel.UnlockedHulls;
			IList<Cruiser> prefabs = new List<Cruiser>();

			foreach (HullKey hullKey in hullKeys)
			{
				Cruiser hull = _prefabFactory.GetCruiserPrefab(hullKey);
				prefabs.Add(hull);
				_hullToKey.Add(hull, hullKey);
			}

			return prefabs;
		}

		public override void SelectUnlockedItem(UnlockedItem<Cruiser> hullItem)
		{
			Cruiser hull = hullItem.Item;
			_gameModel.PlayerLoadout.Hull = _hullToKey[hull];
			
			// Update UI
			_loadoutHull.UpdateHull(hull);
			_unlockedHullsRow.UpdateSelectedHull(hull);
		}
	}
}
