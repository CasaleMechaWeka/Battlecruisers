using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Items;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils.Properties;
using System;


namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public class ProfileCategoryButton : CanvasGroupButton
    {
        private ItemPanelsController _itemPanels;
        public ItemType itemType;
        private ItemFamily ItemFamily => ItemFamily.Profile;
        private IBroadcastingProperty<ItemFamily?> _itemFamilyToCompare;
        private ComparingItemFamilyTracker _itemFamilyTracker;
        public void Initialise(
            SingleSoundPlayer soundPlayer,
            ItemPanelsController itemPanels,
            IBroadcastingProperty<ItemFamily?> itemFamilyToCompare,
            ComparingItemFamilyTracker itemFamilyTracker)
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
            // Profile button is selected when no panel is shown (profile state)
            IsSelected = _itemPanels.CurrentlyShownPanel == null;
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
