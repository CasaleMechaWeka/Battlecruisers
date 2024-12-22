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
        private ItemFamily ItemFamily => ItemFamily.Heckles;
        private IBroadcastingProperty<ItemFamily?> _itemFamilyToCompare;
        private IComparingItemFamilyTracker _itemFamilyTracker;
        public void Initialise(
            ISingleSoundPlayer soundPlayer,
            IItemPanelsController itemPanels,
            IBroadcastingProperty<ItemFamily?> itemFamilyToCompare,
            IComparingItemFamilyTracker itemFamilyTracker)
        {
            base.Initialise(soundPlayer);
            _itemPanels = itemPanels;
            _itemPanels.PotentialMatchChange += _itemPanels_PotentialMatchChange;
            _itemFamilyTracker = itemFamilyTracker;

            _itemFamilyToCompare = itemFamilyToCompare;
            //       _itemFamilyToCompare.ValueChanged += _itemFamilyToCompare_ValueChanged;
        }

        private void _itemPanels_PotentialMatchChange(object sender, EventArgs e)
        {
            UpdateSelectedFeedback();
        }

        private void UpdateSelectedFeedback()
        {
            IsSelected = _itemPanels.IsMatch(itemType);
        }

        public void OnClickedAction() { OnClicked(); }

        protected override void OnClicked()
        {
            base.OnClicked();
            _itemPanels.ShowItemsPanel(itemType);
            _itemFamilyTracker.SetComparingFamily(ItemFamily);
            _itemFamilyTracker.SetComparingFamily(null);
        }

        private void _itemFamilyToCompare_ValueChanged(object sender, EventArgs e)
        {
            Enabled = ShouldBeEnabled();
        }

        private bool ShouldBeEnabled()
        {
            /*            return _hasUnlockedItem
                            && (_itemFamilyToCompare.Value == null
                                || _itemFamilyToCompare.Value == ItemFamily);*/
            return true;
        }
    }
}
