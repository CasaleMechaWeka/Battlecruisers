using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Items;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Properties;
using System;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW
{
    // FELIX  Test :)
    public class ComparisonStateTracker : IComparisonStateTracker
    {
        private readonly IBroadcastingProperty<ItemFamily?> _itemFamilyToCompare;
        private readonly IItemDetailsDisplayer _itemDetailsDisplayer;

        private ComparisonState _state;
        public ComparisonState State
        {
            get { return _state; }
            private set
            {
                if (_state != value)
                {
                    _state = value;

                    if (StateChanged != null)
                    {
                        StateChanged.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }

        public event EventHandler StateChanged;

        public ComparisonStateTracker(IBroadcastingProperty<ItemFamily?> itemFamilyToCompare, IItemDetailsDisplayer itemDetailsDisplayer)
        {
            Helper.AssertIsNotNull(itemFamilyToCompare, itemDetailsDisplayer);
            
            _itemFamilyToCompare = itemFamilyToCompare;
            _itemFamilyToCompare.ValueChanged += _itemFamilyToCompare_ValueChanged;

            _itemDetailsDisplayer = itemDetailsDisplayer;
            _itemDetailsDisplayer.NumOfDetailsShownChanged += _itemDetailsDisplayer_NumOfDetailsShownChanged;

            State = EvaluateState();
        }

        private void _itemFamilyToCompare_ValueChanged(object sender, EventArgs e)
        {
            State = EvaluateState();
        }

        private void _itemDetailsDisplayer_NumOfDetailsShownChanged(object sender, EventArgs e)
        {
            State = EvaluateState();
        }

        private ComparisonState EvaluateState()
        {
            if (_itemFamilyToCompare.Value != null)
            {
                return ComparisonState.ReadyToCompare;
            }
            else if (_itemDetailsDisplayer.NumOfDetailsShown == 2)
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