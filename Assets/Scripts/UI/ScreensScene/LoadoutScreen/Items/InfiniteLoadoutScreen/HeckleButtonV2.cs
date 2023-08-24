using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.UI;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.ShopScreen;
using System;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Models;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Items
{
    public class HeckleButtonV2 : ItemButton
    {
        private RectTransform _selectedFeedback;
        public override IComparableItem Item => null;   // it's really dummy, only for heckles
        private IComparingItemFamilyTracker _itemFamilyTracker;
        private IHeckleData _heckleData;
        private IGameModel _gameModel;
        private ItemsPanel _itemsPanel;

        public SelectHeckleButton selectHeckleButton;
        public Button toggleSelectionButton;

        public override void ShowDetails()
        {
            //  _itemDetailsManager.ShowDetails(null);
            _itemDetailsManager.ShowDetails(_heckleData);
            _itemsPanel.CurrentHeckleButton = this;
        }

        public void Initialise(
            ItemsPanel itemsPanel,
            ISingleSoundPlayer soundPlayer,
            IHeckleData heckleData,
            IItemDetailsManager itemDetailsManager,
            IComparingItemFamilyTracker comparingItemFamily,
            IGameModel gameModel)
        {
            base.Initialise(soundPlayer, itemDetailsManager, comparingItemFamily);
            _itemsPanel = itemsPanel;
            _gameModel = gameModel;
            _selectedFeedback = transform.FindNamedComponent<RectTransform>("SelectedFeedback");
            _heckleData = heckleData;
            _itemsPanel.HeckleButtonChanged += OnCurrentHeckleButtonChanged;
            _itemFamilyTracker = comparingItemFamily;
            _itemFamilyTracker.ComparingFamily.ValueChanged += OnHeckleListChange;
            _itemName.text = Mathf.Max(108, 217 * heckleData.Index).ToString().Substring(0, 3);

            toggleSelectionButton.onClick.AddListener(OnSelectionToggleClicked);

            UpdateSelectedFeedback();
        }

        protected override void OnClicked()
        {
            base.OnClicked();

            _itemsPanel.CurrentHeckleButton = this;
            _comparingFamiltyTracker.SetComparingFamily(itemFamily);
            if (_comparingFamiltyTracker.ComparingFamily.Value == itemFamily)
            {
                _itemDetailsManager.ShowDetails(_heckleData);
                _comparingFamiltyTracker.SetComparingFamily(null);
            }
            else
            {
                //_itemDetailsManager.CompareWithSelectedItem(_unitPrefab.Buildable);
                //_comparingFamiltyTracker.SetComparingFamily(null);
            }
        }


        private void OnCurrentHeckleButtonChanged(object sender, EventArgs e)
        {
            UpdateClickedFeedback = _itemsPanel.CurrentHeckleButton == this;
        }
        private void UpdateSelectedFeedback()
        {
            _selectedFeedback.gameObject.SetActive(_gameModel.PlayerLoadout.CurrentHeckles.Contains(_heckleData.Index));
        }
        private void OnHeckleListChange(object sender, EventArgs e)
        {
            UpdateSelectedFeedback();
        }

        private void OnSelectionToggleClicked()
        {
            if (GetComponentInChildren<ClickedFeedBack>(true).gameObject.activeInHierarchy)
            {
                selectHeckleButton.ToggleHeckleSelection();
                UpdateSelectedFeedback();
            }
        }
    }
}
