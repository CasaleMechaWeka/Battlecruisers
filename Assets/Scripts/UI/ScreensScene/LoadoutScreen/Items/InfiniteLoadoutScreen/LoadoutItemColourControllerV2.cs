using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.Utils;
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Items
{
    /// <summary>
    /// Highlights (changes the colour of) loadout item buttons while the
    /// buttons' item is being displayed in the item details panels.
    /// </summary>
    public class LoadoutItemColourControllerV2
    {
        private readonly IItemDetailsManager _itemDetails;
        private readonly IDictionary<IComparableItem, IItemButton> _itemToButton;

        private IItemButton _selectedButton;
        private IItemButton SelectedButton
        {
            set
            {
                _selectedButton = SetButtonToHighlight(_selectedButton, value);
            }
        }

        private IItemButton _comparingButton;
        private IItemButton ComparingButton
        {
            set
            {
                _comparingButton = SetButtonToHighlight(_comparingButton, value);
            }
        }

        public LoadoutItemColourControllerV2(IItemDetailsManager itemDetails, IList<IItemButton> itemButtons)
        {
            Helper.AssertIsNotNull(itemDetails, itemButtons);

            _itemDetails = itemDetails;
            _itemDetails.SelectedItem.ValueChanged += SelectedItem_ValueChanged;
            _itemDetails.ComparingItem.ValueChanged += ComparingItem_ValueChanged;

            _itemToButton = new Dictionary<IComparableItem, IItemButton>();
            foreach (IItemButton button in itemButtons)
            {
                _itemToButton.Add(button.Item, button);
            }
        }

        private void SelectedItem_ValueChanged(object sender, EventArgs e)
        {
            IComparableItem selectedItem = _itemDetails.SelectedItem.Value;

            if (selectedItem != null)
            {
                Assert.IsTrue(_itemToButton.ContainsKey(selectedItem));
                SelectedButton = _itemToButton[selectedItem];
            }
            else
            {
                SelectedButton = null;
            }
        }

        private void ComparingItem_ValueChanged(object sender, EventArgs e)
        {
            IComparableItem comparedItem = _itemDetails.ComparingItem.Value;

            if (comparedItem != null)
            {
                Assert.IsTrue(_itemToButton.ContainsKey(comparedItem));
                ComparingButton = _itemToButton[comparedItem];
            }
            else
            {
                ComparingButton = null;
            }
        }

        private IItemButton SetButtonToHighlight(IItemButton currentButton, IItemButton newButton)
        {
            if (currentButton != null)
            {
                currentButton.Color = ButtonColour.SelectedNew;
                currentButton.UpdateClickedFeedback = false;
            }

            if (newButton != null)
            {
                newButton.Color = ButtonColour.Selected;
                newButton.UpdateClickedFeedback = true;
            }

            return newButton;
        }
    }
}