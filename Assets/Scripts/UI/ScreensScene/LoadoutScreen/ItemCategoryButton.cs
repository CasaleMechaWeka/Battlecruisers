using BattleCruisers.UI.ScreensScene.LoadoutScreen.Items;
using BattleCruisers.Utils;
using UnityCommon.Properties;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using BattleCruisers.UI.Sound;
using BattleCruisers.Data.Models;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    // FELIX  Make abstract
    public class ItemCategoryButton : ElementWithClickSound, IPointerClickHandler
    {
        private IItemPanelsController _itemPanels;
        private IBroadcastingProperty<ItemFamily?> _itemFamilyToCompare;
        private IGameModel _gameModel;
        private bool _hasUnlockedItem;
        private Image _selectedFeedback;
        private NewItemMark _newItemMark;

        public ItemType itemType;

        // FELIX  Provide from child classes :)
        public ItemFamily itemFamily;

        private bool IsSelected
        {
            set
            {
                _selectedFeedback.enabled = value;
            }
        }

        private CanvasGroup _canvasGroup;
        protected override CanvasGroup CanvasGroup => _canvasGroup;

        public void Initialise(
            ISoundPlayer soundPlayer, 
            IItemPanelsController itemPanels, 
            IBroadcastingProperty<ItemFamily?> itemFamilyToCompare,
            IGameModel gameModel)
        {
            base.Initialise(soundPlayer);

            Helper.AssertIsNotNull(itemPanels, itemFamilyToCompare, gameModel);

            _itemPanels = itemPanels;
            _itemPanels.PotentialMatchChange += _itemPanels_PotentialMatchChange;

            _itemFamilyToCompare = itemFamilyToCompare;
            _itemFamilyToCompare.ValueChanged += _itemFamilyToCompare_ValueChanged;

            _gameModel = gameModel;
            _hasUnlockedItem = itemPanels.GetPanel(itemType).HasUnlockedItem;

            _canvasGroup = GetComponent<CanvasGroup>();
            Assert.IsNotNull(_canvasGroup);

            _selectedFeedback = transform.FindNamedComponent<Image>("SelectedFeedback");
            UpdateSelectedFeedback();

            _newItemMark = GetComponentInChildren<NewItemMark>();
            Assert.IsNotNull(_newItemMark);
            SetupNewMarkVisibilityCallback(_gameModel);
            UpdateNewItemMarkVisibility();

            Enabled = ShouldBeEnabled();
        }

        private void _itemPanels_PotentialMatchChange(object sender, EventArgs e)
        {
            UpdateSelectedFeedback();
        }

        private void _itemFamilyToCompare_ValueChanged(object sender, EventArgs e)
        {
            Enabled = ShouldBeEnabled();
        }

        private bool ShouldBeEnabled()
        {
            return _hasUnlockedItem
                && (_itemFamilyToCompare.Value == null
                    || _itemFamilyToCompare.Value == itemFamily);
        }

        private void UpdateSelectedFeedback()
        {
            IsSelected = _itemPanels.IsMatch(itemType);
        }

        protected override void OnClicked()
        {
            base.OnClicked();
            _itemPanels.ShowItemsPanel(itemType);
        }

        protected abstract void SetupNewMarkVisibilityCallback(IGameModel gameModel);
        protected abstract bool IsNew(IGameModel gameModel);

        protected void UpdateNewItemMarkVisibility()
        {
            _newItemMark.IsVisible = IsNew(_gameModel);
        }
    }
}