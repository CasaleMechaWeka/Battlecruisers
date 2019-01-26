using BattleCruisers.Buildables;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.ItemDetails;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Properties;
using System;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW
{
    public class CompareButton : Togglable, IPointerClickHandler
    {
        private IItemDetailsDisplayer _itemDetailsDisplayer;
        private IBroadcastingProperty<TargetType?> _itemTypeToCompare;

        protected override bool Disable { get { return true; } }

        public void Initialise(IItemDetailsDisplayer itemDetailsDisplayer, IBroadcastingProperty<TargetType?> itemToCompareTracker)
        {
            Helper.AssertIsNotNull(itemDetailsDisplayer, itemToCompareTracker);

            _itemDetailsDisplayer = itemDetailsDisplayer;
            _itemTypeToCompare = itemToCompareTracker;

            _itemTypeToCompare.ValueChanged += _itemTypeToCompare_ValueChanged;
        }

        private void _itemTypeToCompare_ValueChanged(object sender, EventArgs e)
        {
            Enabled = _itemTypeToCompare.Value == null;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _itemTypeToCompare.Value = _itemDetailsDisplayer.SelectedItemType;
        }
    }
}