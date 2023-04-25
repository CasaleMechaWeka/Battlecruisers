using BattleCruisers.Data.Models;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Items;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using System;
using System.Collections.Generic;
using BattleCruisers.Utils.Properties;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;

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
        private RectTransform _itemCategoryButton;

        private IComparingItemFamilyTracker _itemFamilyTracker;

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
            IList<IItemButton> itemButtons,
            IComparingItemFamilyTracker itemFamilyTracker)
        {
            base.Initialise(soundPlayer);

            Helper.AssertIsNotNull(itemPanels, itemFamilyToCompare, gameModel, itemButtons);

            _itemPanels = itemPanels;
            _itemPanels.PotentialMatchChange += _itemPanels_PotentialMatchChange;

            
            _itemFamilyTracker = itemFamilyTracker;

            _itemFamilyToCompare = itemFamilyToCompare;
            _itemFamilyToCompare.ValueChanged += _itemFamilyToCompare_ValueChanged;

            _gameModel = gameModel;
            _hasUnlockedItem = itemPanels.GetPanel(itemType).HasUnlockedItem;

            _canvasGroup = GetComponent<CanvasGroup>();
            Assert.IsNotNull(_canvasGroup);

            _selectedFeedback = transform.FindNamedComponent<Transform>("SelectedFeedback").gameObject;
            _itemCategoryButton = GetComponent<RectTransform>();
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
            if (_itemPanels.IsMatch(itemType))
            {
                _itemCategoryButton.sizeDelta = new Vector2(300, 150);
            }
            else
            {
                _itemCategoryButton.sizeDelta = new Vector2(150, 150);
            }
            IsSelected = _itemPanels.IsMatch(itemType);
        }

        protected override void OnClicked()
        {
            base.OnClicked();
            _itemFamilyTracker.SetComparingFamily(ItemFamily);
            _itemPanels.ShowItemsPanel(itemType);
            _itemFamilyTracker.SetComparingFamily(null);
        }

        protected abstract void SetupNewMarkVisibilityCallback(IGameModel gameModel);
        protected abstract bool HasNewItems(IGameModel gameModel);

        protected void UpdateNewItemMarkVisibility()
        {
            if (_newItemMark == null)
            {
                return;
            }
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