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

        protected override bool ToggleVisibility { get { return true; } }

        public void Initialise(IItemDetailsDisplayer itemDetailsDisplayer, IBroadcastingProperty<ItemFamily?> itemFamilyToCompare)
        {
            base.Initialise();

            Helper.AssertIsNotNull(itemDetailsDisplayer, itemFamilyToCompare);

            _itemDetailsDisplayer = itemDetailsDisplayer;
            _itemFamilyToCompare = itemFamilyToCompare;

            _itemFamilyToCompare.ValueChanged += _itemFamilyToCompare_ValueChanged;
        }

        private void _itemFamilyToCompare_ValueChanged(object sender, EventArgs e)
        {
            Enabled = _itemFamilyToCompare.Value == null;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _itemFamilyToCompare.Value = _itemDetailsDisplayer.SelectedItemFamily;
        }
    }
}