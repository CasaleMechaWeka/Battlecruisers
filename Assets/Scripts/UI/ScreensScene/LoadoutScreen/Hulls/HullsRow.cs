using System;
using System.Collections.Generic;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data;
using BattleCruisers.Data.PrefabKeys;
using BattleCruisers.Fetchers;
using BattleCruisers.Cruisers;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Hulls
{
	public class HullsRow
	{
		private readonly IGameModel _gameModel;
		private readonly IPrefabFactory _prefabFactory;
		private readonly LoadoutHullItem _loadoutHull;
		private readonly UnlockedHullsRow _unlockedHullsRow;
		private readonly IDictionary<Cruiser, HullKey> _hullToKey;

		public HullsRow(IGameModel gameModel, IPrefabFactory prefabFactory, IUIFactory uiFactory, LoadoutHullItem loadoutHull, UnlockedHullsRow unlockedHullsRow)
		{
			_gameModel = gameModel;
			_prefabFactory = prefabFactory;
			_loadoutHull = loadoutHull;
			_unlockedHullsRow = unlockedHullsRow;

			_hullToKey = new Dictionary<Cruiser, HullKey>();

			Cruiser loadoutCruiser = _prefabFactory.GetCruiserPrefab(_gameModel.PlayerLoadout.Hull);
			_loadoutHull.Sprite = loadoutCruiser.Sprite;
			_unlockedHullsRow.Initialise(this, uiFactory, GetUnlockedHullPrefabs(), loadoutCruiser);
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

		public void SelectHull(Cruiser hull)
		{
			_gameModel.PlayerLoadout.Hull = _hullToKey[hull];

			// Update UI
			_loadoutHull.Sprite = hull.Sprite;
			_unlockedHullsRow.UpdateSelectedHull(hull);
		}
	}
}
