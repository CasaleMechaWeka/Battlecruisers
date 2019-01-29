using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Items;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Properties;
using System;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW
{
    public class ComparisonStateTracker : IComparisonStateTracker
    {
        private readonly IBroadcastingProperty<ItemFamily?> _itemFamilyToCompare;
        private readonly IItemDetailsManager _itemDetailsManager;

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

        public ComparisonStateTracker(IBroadcastingProperty<ItemFamily?> itemFamilyToCompare, IItemDetailsManager itemDetailsManager)
        {
            Helper.AssertIsNotNull(itemFamilyToCompare, itemDetailsManager);
            
            _itemFamilyToCompare = itemFamilyToCompare;
            _itemFamilyToCompare.ValueChanged += _itemFamilyToCompare_ValueChanged;

            _itemDetailsManager = itemDetailsManager;
            _itemDetailsManager.NumOfDetailsShownChanged += _itemDetailsManager_NumOfDetailsShownChanged;

            State = EvaluateState();
        }

        private void _itemFamilyToCompare_ValueChanged(object sender, EventArgs e)
        {
            State = EvaluateState();
        }

        private void _itemDetailsManager_NumOfDetailsShownChanged(object sender, EventArgs e)
        {
            State = EvaluateState();
        }

        private ComparisonState EvaluateState()
        {
            if (_itemFamilyToCompare.Value != null)
            {
                return ComparisonState.ReadyToCompare;
            }
            else if (_itemDetailsManager.NumOfDetailsShown == 2)
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