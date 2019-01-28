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
        private IItemDetailsDisplayer _itemDetailsDisplayer;
        private IBroadcastingProperty<ItemFamily?> _itemFamilyToCompare;
        private IComparisonStateTracker _comparisonStateTracker;

        protected override bool ToggleVisibility { get { return true; } }

        public void Initialise(
            IItemDetailsDisplayer itemDetailsDisplayer, 
            IBroadcastingProperty<ItemFamily?> itemFamilyToCompare,
            IComparisonStateTracker comparisonStateTracker)
        {
            base.Initialise();

            Helper.AssertIsNotNull(itemDetailsDisplayer, itemFamilyToCompare, comparisonStateTracker);

            _itemDetailsDisplayer = itemDetailsDisplayer;
            _itemFamilyToCompare = itemFamilyToCompare;
            _comparisonStateTracker = comparisonStateTracker;

            _comparisonStateTracker.StateChanged += _comparisonStateTracker_StateChanged;
        }

        // FELIX  Don't show while comparing, otherwise is visible above left item details :P
        // FELIX Use generic FilterToggler instead.  Extracts this logic to implementation of IBroadcastingFilter.
        private void _comparisonStateTracker_StateChanged(object sender, EventArgs e)
        {
            Enabled = _comparisonStateTracker.State == ComparisonState.NotComparing;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _itemFamilyToCompare.Value = _itemDetailsDisplayer.SelectedItemFamily;
        }
    }
}