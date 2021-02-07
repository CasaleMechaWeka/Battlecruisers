using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons
{
    public class CompareButton : CanvasGroupButton
    {
        private IItemDetailsManager _itemDetailsManager;
        private IComparingItemFamilyTracker _comparingFamilyTracker;
        private IComparisonStateTracker _comparisonStateTracker;

        protected override bool ToggleVisibility => true;

        public void Initialise(
            ISingleSoundPlayer soundPlayer,
            IItemDetailsManager itemDetailsManager,
            IComparingItemFamilyTracker comparingFamilyTracker,
            IComparisonStateTracker comparisonStateTracker)
        {
            base.Initialise(soundPlayer);

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

        protected override void OnClicked()
        {
            base.OnClicked();
            _comparingFamilyTracker.SetComparingFamily(_itemDetailsManager.SelectedItemFamily);
        }
    }
}