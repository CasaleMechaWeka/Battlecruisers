using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Items;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Properties;
using System;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW
{
    public class CompareButton : Togglable, IPointerClickHandler
    {
        private IItemDetailsManager _itemDetailsManager;
        private ISettableBroadcastingProperty<ItemFamily?> _itemFamilyToCompare;
        private IComparisonStateTracker _comparisonStateTracker;

        protected override bool ToggleVisibility { get { return true; } }

        public void Initialise(
            IItemDetailsManager itemDetailsManager, 
            ISettableBroadcastingProperty<ItemFamily?> itemFamilyToCompare,
            IComparisonStateTracker comparisonStateTracker)
        {
            base.Initialise();

            Helper.AssertIsNotNull(itemDetailsManager, itemFamilyToCompare, comparisonStateTracker);

            _itemDetailsManager = itemDetailsManager;
            _itemFamilyToCompare = itemFamilyToCompare;
            _comparisonStateTracker = comparisonStateTracker;

            _comparisonStateTracker.State.ValueChanged += State_ValueChanged;
        }

        private void State_ValueChanged(object sender, EventArgs e)
        {
            Enabled = _comparisonStateTracker.State.Value == ComparisonState.NotComparing;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _itemFamilyToCompare.Value = _itemDetailsManager.SelectedItemFamily;
        }
    }
}