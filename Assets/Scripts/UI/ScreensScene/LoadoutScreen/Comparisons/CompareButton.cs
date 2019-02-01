using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.Utils;
using System;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons
{
    public class CompareButton : Togglable, IPointerClickHandler
    {
        private IItemDetailsManager _itemDetailsManager;
        private IComparingItemFamilyTracker _comparingFamilyTracker;
        private IComparisonStateTracker _comparisonStateTracker;

        protected override bool ToggleVisibility { get { return true; } }

        public void Initialise(
            IItemDetailsManager itemDetailsManager,
            IComparingItemFamilyTracker comparingFamilyTracker,
            IComparisonStateTracker comparisonStateTracker)
        {
            base.Initialise();

            Helper.AssertIsNotNull(itemDetailsManager, comparingFamilyTracker, comparisonStateTracker);

            _itemDetailsManager = itemDetailsManager;
            _comparingFamilyTracker = comparingFamilyTracker;
            _comparisonStateTracker = comparisonStateTracker;

            _comparisonStateTracker.State.ValueChanged += State_ValueChanged;
        }

        private void State_ValueChanged(object sender, EventArgs e)
        {
            Enabled = _comparisonStateTracker.State.Value == ComparisonState.NotComparing;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _comparingFamilyTracker.SetComparingFamily(_itemDetailsManager.SelectedItemFamily);
        }
    }
}