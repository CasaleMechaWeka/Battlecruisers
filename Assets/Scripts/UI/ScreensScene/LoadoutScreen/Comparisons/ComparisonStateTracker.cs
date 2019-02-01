using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Items;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Properties;
using System;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons
{
    public class ComparisonStateTracker : IComparisonStateTracker
    {
        private readonly IBroadcastingProperty<ItemFamily?> _itemFamilyToCompare;
        private readonly IItemDetailsManager _itemDetailsManager;

        private readonly ISettableBroadcastingProperty<ComparisonState> _state;
        public IBroadcastingProperty<ComparisonState> State { get; private set; }

        public ComparisonStateTracker(IBroadcastingProperty<ItemFamily?> itemFamilyToCompare, IItemDetailsManager itemDetailsManager)
        {
            Helper.AssertIsNotNull(itemFamilyToCompare, itemDetailsManager);
            
            _itemFamilyToCompare = itemFamilyToCompare;
            _itemFamilyToCompare.ValueChanged += _itemFamilyToCompare_ValueChanged;

            _itemDetailsManager = itemDetailsManager;
            _itemDetailsManager.NumOfDetailsShown.ValueChanged += _itemDetailsManager_NumOfDetailsShownChanged;

            _state = new SettableBroadcastingProperty<ComparisonState>(initialValue: ComparisonState.NotComparing);
            State = new BroadcastingProperty<ComparisonState>(_state);

            _state.Value = EvaluateState();
        }

        private void _itemFamilyToCompare_ValueChanged(object sender, EventArgs e)
        {
            _state.Value = EvaluateState();
        }

        private void _itemDetailsManager_NumOfDetailsShownChanged(object sender, EventArgs e)
        {
            _state.Value = EvaluateState();
        }

        private ComparisonState EvaluateState()
        {
            if (_itemFamilyToCompare.Value != null)
            {
                return ComparisonState.ReadyToCompare;
            }
            else if (_itemDetailsManager.NumOfDetailsShown.Value == 2)
            {
                return ComparisonState.Comparing;
            }
            else
            {
                return ComparisonState.NotComparing;
            }
        }
    }
}