using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.LoadoutItems
{
    public abstract class LoadoutBuildableItemsRow<TBuildable, TPrefabKey> : MonoBehaviour, IStatefulUIElement
        where TBuildable : IBuildable
        where TPrefabKey : IPrefabKey
	{
        protected IGameModel _gameModel;
        protected ILockedInformation _lockedInfo;
        protected IPrefabFactory _prefabFactory;
        protected IUIFactory _uiFactory;
        private IItemDetailsManager<TBuildable> _detailsManager;
        private IDictionary<TBuildable, LoadoutItem<TBuildable>> _buildableToLoadoutItem;
        protected HorizontalLayoutGroup _layoutGroup;

        private const int MAX_NUM_OF_ITEMS = 5;

        protected abstract int NumOfLockedBuildables { get; }

        // FELIX  Create subclass/interface, becase we won't need the details manager soon?
        public void Initialise(IItemsRowArgs<TBuildable> args)
		{
            Helper.AssertIsNotNull(args);

            _gameModel = args.GameModel;
            _lockedInfo = args.LockedInfo;
            _prefabFactory = args.PrefabFactory;
			_uiFactory = args.UIFactory;
			_detailsManager = args.DetailsManager;
            _buildableToLoadoutItem = new Dictionary<TBuildable, LoadoutItem<TBuildable>>();

            _layoutGroup = GetComponent<HorizontalLayoutGroup>();
            Assert.IsNotNull(_layoutGroup);

            _detailsManager.StateChanged += _detailsManager_StateChanged;

        }

        // Not in Initialise() because uses abstract members which will only
        // be correct after Iniitalise().
        public void SetupUI()
        {
            IList<TBuildable> _buildables = GetLoadoutBuildablePrefabs();

            Assert.IsTrue(_buildables.Count + NumOfLockedBuildables <= MAX_NUM_OF_ITEMS);

            // Create unlocked items
			foreach (TBuildable buildable in _buildables)
			{
				CreateLoadoutItem(buildable);
			}

            // Create placeholders for locked items
            for (int i = 0; i < NumOfLockedBuildables; ++i)
            {
                _uiFactory.CreateLockedBuildable(_layoutGroup);
            }
        }

        protected abstract IList<TBuildable> GetLoadoutBuildablePrefabs();

        private void CreateLoadoutItem(TBuildable buildableToAdd)
        {
            Assert.IsFalse(_buildableToLoadoutItem.ContainsKey(buildableToAdd));
            LoadoutItem<TBuildable> item = CreateItem(buildableToAdd);
            _buildableToLoadoutItem.Add(buildableToAdd, item);
        }

        protected abstract LoadoutItem<TBuildable> CreateItem(TBuildable item);

        protected IList<TBuildable> GetBuildablePrefabs(IList<TPrefabKey> buildableKeys)
        {
            IList<TBuildable> prefabs = new List<TBuildable>();

            foreach (TPrefabKey key in buildableKeys)
            {
                TBuildable buildable = GetBuildablePrefab(key);
                prefabs.Add(buildable);
            }

            return prefabs;
        }

        protected abstract TBuildable GetBuildablePrefab(TPrefabKey prefabKey);

        // FELIX  Will be replaced :)
        private void _detailsManager_StateChanged(object sender, StateChangedEventArgs<TBuildable> e)
		{
            foreach (LoadoutItem<TBuildable> item in _buildableToLoadoutItem.Values)
			{
				item.backgroundImage.color = e.NewState.IsInReadyToCompareState ? BaseItem<Building>.Colors.HIGHLIGHTED : BaseItem<Building>.Colors.DEFAULT;
			}
		}

        public void GoToState(UIState state)
        {
            // FELIX
            throw new System.NotImplementedException();
        }
	}
}
