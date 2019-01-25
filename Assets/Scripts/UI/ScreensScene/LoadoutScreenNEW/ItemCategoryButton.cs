using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Items;
using BattleCruisers.Utils;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW
{
    // FELIX  Handle being disabled during comparison.  Follow BuildableButtonController's use of IBroadcastingFilter??
    public class ItemCategoryButton : MonoBehaviour, IPointerClickHandler
    {
        private IItemPanelsController _itemPanels;
        private Image _selectedFeedback;

        public ItemType itemType;

        private bool IsSelected
        {
            set
            {
                _selectedFeedback.enabled = value;
            }
        }

        public void Initialise(IItemPanelsController itemPanels)
        {
            Assert.IsNotNull(itemPanels);

            _itemPanels = itemPanels;
            _itemPanels.PotentialMatchChange += _itemPanels_PotentialMatchChange;

            _selectedFeedback = transform.FindNamedComponent<Image>("SelectedFeedback");
            UpdateSelectedFeedback();
        }

        private void _itemPanels_PotentialMatchChange(object sender, EventArgs e)
        {
            UpdateSelectedFeedback();
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