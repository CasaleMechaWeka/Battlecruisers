using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.LoadoutItems;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.UnlockedItems;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows
{
    public class HullItemsRow : ItemsRow<ICruiser>, IHullItemsRow
    {
		private readonly LoadoutHullItem _loadoutHull;
        private readonly UnlockedHullItemsRow _unlockedHullsRow;
		private readonly IDictionary<ICruiser, HullKey> _hullToKey;

        public HullKey UserChosenHull { get; private set; }

        public HullItemsRow(
            IItemsRowArgs<ICruiser> args,
            LoadoutHullItem loadoutHull, 
            UnlockedHullItemsRow unlockedHullsRow)
            : base(args)
		{
            Helper.AssertIsNotNull(loadoutHull, unlockedHullsRow);

			_loadoutHull = loadoutHull;
            _unlockedHullsRow = unlockedHullsRow;

            _hullToKey = CreateHullToKeyMap();
        }

        private IDictionary<ICruiser, HullKey> CreateHullToKeyMap()
        {
            IDictionary<ICruiser, HullKey> hullToKey = new Dictionary<ICruiser, HullKey>();

            foreach (HullKey hullKey in _dataProvider.GameModel.UnlockedHulls)
            {
                ICruiser hull = _prefabFactory.GetCruiserPrefab(hullKey);
                hullToKey.Add(hull, hullKey);
            }

            return hullToKey;
        }

        public override void SetupUI()
        {
			Cruiser loadoutCruiser = _prefabFactory.GetCruiserPrefab(_dataProvider.GameModel.PlayerLoadout.Hull);
            _loadoutHull.Initialise(loadoutCruiser, _detailsManager);

            _unlockedHullsRow.Initialise(_detailsManager, this, _dataProvider, _prefabFactory);
        }

		public override bool SelectUnlockedItem(UnlockedItem<ICruiser> hullItem)
		{
            UpdateUserChosenHull(hullItem.Item);
			return true;
		}

        public override void GoToState(UIState state)
        {
            _loadoutHull.GoToState(state);
            _unlockedHullsRow.GoToState(state);
        }

        public void OnPresenting(object activationParameter)
        {
            ICruiser loadoutHull = HullForKey(_dataProvider.GameModel.PlayerLoadout.Hull);
            UpdateUserChosenHull(loadoutHull);
            _unlockedHullsRow.RefreshLockedStatus();
        }

        private ICruiser HullForKey(HullKey hullKey)
        {
            Assert.IsTrue(_hullToKey.Count != 0);

            ICruiser hull =
                _hullToKey
                    .FirstOrDefault(hullToKey => hullToKey.Value.Equals(hullKey))
                    .Key;
            Assert.IsNotNull(hull);

            return hull;
        }

        private void UpdateUserChosenHull(ICruiser hull)
        {
            UserChosenHull = _hullToKey[hull];

            // Update UI
            _loadoutHull.UpdateHull(hull);
            _unlockedHullsRow.UpdateSelectedHull(hull);
        }

        public void OnDismissing()
        {
            // empty
        }
    }
}
