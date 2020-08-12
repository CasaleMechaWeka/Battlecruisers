using BattleCruisers.UI.ScreensScene.LoadoutScreen.Items;
using BattleCruisers.Utils;
using UnityCommon.Properties;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using BattleCruisers.UI.Sound;
using BattleCruisers.Data.Models;
using System.Collections.Generic;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public abstract class ItemCategoryButton : ElementWithClickSound, IPointerClickHandler, IManagedDisposable
    {
        private IItemPanelsController _itemPanels;
        private IBroadcastingProperty<ItemFamily?> _itemFamilyToCompare;
        private IGameModel _gameModel;
        private bool _hasUnlockedItem;
        private GameObject _selectedFeedback;
        private NewItemMark _newItemMark;

        public ItemType itemType;

        protected abstract ItemFamily ItemFamily { get; }

        private bool IsSelected
        {
            set
            {
                _selectedFeedback.SetActive(value);
            }
        }

        private CanvasGroup _canvasGroup;
        protected override CanvasGroup CanvasGroup => _canvasGroup;

        public void Initialise(
            ISingleSoundPlayer soundPlayer, 
            IItemPanelsController itemPanels, 
            IBroadcastingProperty<ItemFamily?> itemFamilyToCompare,
            IGameModel gameModel,
            IList<IItemButton> itemButtons)
        {
            base.Initialise(soundPlayer);

            Helper.AssertIsNotNull(itemPanels, itemFamilyToCompare, gameModel, itemButtons);

            _itemPanels = itemPanels;
            _itemPanels.PotentialMatchChange += _itemPanels_PotentialMatchChange;

            _itemFamilyToCompare = itemFamilyToCompare;
            _itemFamilyToCompare.ValueChanged += _itemFamilyToCompare_ValueChanged;

            _gameModel = gameModel;
            _hasUnlockedItem = itemPanels.GetPanel(itemType).HasUnlockedItem;

            _canvasGroup = GetComponent<CanvasGroup>();
            Assert.IsNotNull(_canvasGroup);

            _selectedFeedback = transform.FindNamedComponent<Transform>("SelectedFeedback").gameObject;
            UpdateSelectedFeedback();

            _newItemMark = GetComponentInChildren<NewItemMark>();
            Assert.IsNotNull(_newItemMark);
            SetupNewMarkVisibilityCallback(_gameModel);
            UpdateNewItemMarkVisibility();

            foreach (IItemButton button in itemButtons)
            {
                button.Clicked += (sender, e) => UpdateNewItemMarkVisibility();
            }

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
                    || _itemFamilyToCompare.Value == ItemFamily);
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
        protected abstract bool HasNewItems(IGameModel gameModel);

        protected void UpdateNewItemMarkVisibility()
        {
            Logging.Log(Tags.LOADOUT_SCREEN, $"_newItemMark.IsVisible: {_newItemMark.IsVisible}");
            _newItemMark.IsVisible = HasNewItems(_gameModel);
            Logging.Log(Tags.LOADOUT_SCREEN, $"_newItemMark.IsVisible: {_newItemMark.IsVisible}");
        }

        public void DisposeManagedState()
        {
            CleanUp(_gameModel);
        }

        protected abstract void CleanUp(IGameModel gameModel);
    }
}