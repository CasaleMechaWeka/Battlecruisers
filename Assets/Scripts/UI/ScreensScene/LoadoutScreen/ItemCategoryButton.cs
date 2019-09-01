using BattleCruisers.UI.ScreensScene.LoadoutScreen.Items;
using BattleCruisers.Utils;
using UnityCommon.Properties;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using BattleCruisers.UI.Sound;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public class ItemCategoryButton : ButtonWithClickSound, IPointerClickHandler
    {
        private IItemPanelsController _itemPanels;
        private IBroadcastingProperty<ItemFamily?> _itemFamilyToCompare;
        private bool _hasUnlockedItem;
        private Image _selectedFeedback;

        public ItemType itemType;
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

        public void Initialise(ISoundPlayer soundPlayer, IItemPanelsController itemPanels, IBroadcastingProperty<ItemFamily?> itemFamilyToCompare)
        {
            base.Initialise(soundPlayer);

            Helper.AssertIsNotNull(itemPanels, itemFamilyToCompare);

            _itemPanels = itemPanels;
            _itemPanels.PotentialMatchChange += _itemPanels_PotentialMatchChange;

            _itemFamilyToCompare = itemFamilyToCompare;
            _itemFamilyToCompare.ValueChanged += _itemFamilyToCompare_ValueChanged;

            _hasUnlockedItem = itemPanels.GetPanel(itemType).HasUnlockedItem;

            _canvasGroup = GetComponent<CanvasGroup>();
            Assert.IsNotNull(_canvasGroup);

            _selectedFeedback = transform.FindNamedComponent<Image>("SelectedFeedback");
            UpdateSelectedFeedback();

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
    }
}