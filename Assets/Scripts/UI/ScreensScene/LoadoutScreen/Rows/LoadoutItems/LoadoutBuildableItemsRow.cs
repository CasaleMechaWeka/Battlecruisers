using BattleCruisers.Buildables;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.BattleScene.Presentables;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.LoadoutItems
{
    public abstract class LoadoutBuildableItemsRow<TBuildable, TPrefabKey> : PresentableController, IStatefulUIElement
        where TBuildable : class, IBuildable
        where TPrefabKey : class, IPrefabKey
	{
        private IList<LoadoutItemWrapper<TBuildable, TPrefabKey>> _buildableItems;

        private const int MAX_NUM_OF_ITEMS = 5;

        public void Initialise(IItemsRowArgs<TBuildable> args)
		{
            base.Initialise();

            Helper.AssertIsNotNull(args);

            _buildableItems = GetComponentsInChildren<LoadoutItemWrapper<TBuildable, TPrefabKey>>().ToList();
            IList<TPrefabKey> buildableKeys = FindBuildableKeys(args.DataProvider.StaticData);

            Assert.AreEqual(buildableKeys.Count, _buildableItems.Count);
            Assert.IsTrue(buildableKeys.Count <= MAX_NUM_OF_ITEMS);

            for (int i = 0; i < _buildableItems.Count; ++i)
            {
                LoadoutItemWrapper<TBuildable, TPrefabKey> buildableItem = _buildableItems[i];
                TPrefabKey buildableKey = buildableKeys[i];
                TBuildable buildablePrefab = GetBuildablePrefab(args.PrefabFactory, buildableKey);

                buildableItem.Initialise(buildablePrefab, buildableKey, args.DetailsManager, args.DataProvider.GameModel);
                AddChildPresentable(buildableItem);
            }
        }

        protected abstract IList<TPrefabKey> FindBuildableKeys(IStaticData staticData);

        protected abstract TBuildable GetBuildablePrefab(IPrefabFactory prefabFactory, TPrefabKey buildableKey);

        public void GoToState(UIState state)
        {
            foreach (LoadoutItemWrapper<TBuildable, TPrefabKey> buildableItem in _buildableItems)
            {
                buildableItem.GoToState(state);
            }
        }
	}
}
