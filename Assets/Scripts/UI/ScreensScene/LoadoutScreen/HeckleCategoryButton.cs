using BattleCruisers.Data.Models;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Items;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public class HeckleCategoryButton : CanvasGroupButton
    {
        private IItemPanelsController _itemPanels;
        public ItemType itemType;
        public void Initialise(
            ISingleSoundPlayer soundPlayer,
            IItemPanelsController itemPanels)
        {
            base.Initialise(soundPlayer);
            _itemPanels = itemPanels;
            _itemPanels.PotentialMatchChange += _itemPanels_PotentialMatchChange;
        }

        private void _itemPanels_PotentialMatchChange(object sender, EventArgs e)
        {
            UpdateSelectedFeedback();
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
