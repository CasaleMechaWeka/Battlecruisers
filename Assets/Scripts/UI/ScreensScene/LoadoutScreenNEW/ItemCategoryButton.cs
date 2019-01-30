using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Items;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Properties;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW
{
    public class ItemCategoryButton : Togglable, IPointerClickHandler
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
        protected override CanvasGroup CanvasGroup { get { return _canvasGroup; } }

        public void Initialise(IItemPanelsController itemPanels, IBroadcastingProperty<ItemFamily?> itemFamilyToCompare)
        {
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

        public void OnPointerClick(PointerEventData eventData)
        {
            _itemPanels.ShowItemsPanel(itemType);
        }
    }
}