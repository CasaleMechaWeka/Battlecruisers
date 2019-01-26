using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Items;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Properties;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW
{
    // FELIX  Show disabled feedback.  Make text opaque?  CanvasGroup?
    public class ItemCategoryButton : Togglable, IPointerClickHandler
    {
        private IItemPanelsController _itemPanels;
        private IBroadcastingProperty<ItemFamily?> _itemFamilyToCompare;
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

        public void Initialise(IItemPanelsController itemPanels, IBroadcastingProperty<ItemFamily?> itemFamilyToCompare)
        {
            Helper.AssertIsNotNull(itemPanels, itemFamily);

            _itemPanels = itemPanels;
            _itemPanels.PotentialMatchChange += _itemPanels_PotentialMatchChange;

            _itemFamilyToCompare = itemFamilyToCompare;
            _itemFamilyToCompare.ValueChanged += _itemFamilyToCompare_ValueChanged;

            _selectedFeedback = transform.FindNamedComponent<Image>("SelectedFeedback");
            UpdateSelectedFeedback();
        }

        private void _itemPanels_PotentialMatchChange(object sender, EventArgs e)
        {
            UpdateSelectedFeedback();
        }

        private void _itemFamilyToCompare_ValueChanged(object sender, EventArgs e)
        {
            Enabled = _itemFamilyToCompare.Value == itemFamily;
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