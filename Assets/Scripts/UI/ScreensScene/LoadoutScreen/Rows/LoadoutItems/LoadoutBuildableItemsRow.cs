using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.LoadoutItems
{
    public abstract class LoadoutBuildableItemsRow<TBuildable> : MonoBehaviour where TBuildable : IBuildable
	{
		protected IUIFactory _uiFactory;
        private IList<TBuildable> _buildables;
        private IItemDetailsManager<TBuildable> _detailsManager;
        private IDictionary<TBuildable, LoadoutItem<TBuildable>> _buildableToLoadoutItem;
        protected HorizontalLayoutGroup _layoutGroup;

        private const int MAX_NUM_OF_ITEMS = 5;

        public void Initialise(IUIFactory uiFactory, IList<TBuildable> buildables, IItemDetailsManager<TBuildable> detailsManager)
		{
            Helper.AssertIsNotNull(uiFactory, buildables, detailsManager);
			Assert.IsTrue(buildables.Count <= MAX_NUM_OF_ITEMS);

			_uiFactory = uiFactory;
            _buildables = buildables;
			_detailsManager = detailsManager;
            _buildableToLoadoutItem = new Dictionary<TBuildable, LoadoutItem<TBuildable>>();

            _layoutGroup = GetComponent<HorizontalLayoutGroup>();
            Assert.IsNotNull(_layoutGroup);

            _detailsManager.StateChanged += _detailsManager_StateChanged;
        }

        public void SetupUI()
        {
			foreach (TBuildable buildable in _buildables)
			{
				CreateLoadoutItem(buildable);
			}
        }

        private void _detailsManager_StateChanged(object sender, StateChangedEventArgs<TBuildable> e)
		{
            foreach (LoadoutItem<TBuildable> item in _buildableToLoadoutItem.Values)
			{
				item.backgroundImage.color = e.NewState.IsInReadyToCompareState ? BaseItem<Building>.Colors.ENABLED : BaseItem<Building>.Colors.DEFAULT;
			}
		}

		public bool CanAddBuildable()
		{
			return _buildableToLoadoutItem.Count < MAX_NUM_OF_ITEMS;
		}

        public void AddBuildable(TBuildable buildableToAdd)
		{
			CreateLoadoutItem(buildableToAdd);
		}

        public void RemoveBuildable(TBuildable buildableToRemove)
		{
			RemoveLoadoutItem(buildableToRemove);
		}
		
        private void CreateLoadoutItem(TBuildable buildableToAdd)
		{
			Assert.IsFalse(_buildableToLoadoutItem.ContainsKey(buildableToAdd));
            LoadoutItem<TBuildable> item = CreateItem(buildableToAdd);
			_buildableToLoadoutItem.Add(buildableToAdd, item);
		}

        protected abstract LoadoutItem<TBuildable> CreateItem(TBuildable item);

        private void RemoveLoadoutItem(TBuildable buildableToRemove)
		{
			Assert.IsTrue(_buildableToLoadoutItem.ContainsKey(buildableToRemove));
            LoadoutItem<TBuildable> item = _buildableToLoadoutItem[buildableToRemove];
			_buildableToLoadoutItem.Remove(buildableToRemove);
			Destroy(item.gameObject);
		}
	}
}
