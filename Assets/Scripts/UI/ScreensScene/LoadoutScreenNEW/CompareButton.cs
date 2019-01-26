using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.ItemDetails;
using BattleCruisers.Utils;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW
{
    public class CompareButton : Togglable, IPointerClickHandler
    {
        private IItemDetailsDisplayer _itemDetailsDisplayer;
        private IItemToCompareTracker _itemToCompareTracker;

        protected override bool Disable { get { return true; } }

        public void Initialise(IItemDetailsDisplayer itemDetailsDisplayer, IItemToCompareTracker itemToCompareTracker)
        {
            Helper.AssertIsNotNull(itemDetailsDisplayer, itemToCompareTracker);

            _itemDetailsDisplayer = itemDetailsDisplayer;
            _itemToCompareTracker = itemToCompareTracker;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _itemToCompareTracker.ItemTypeToCompare = _itemDetailsDisplayer.SelectedItemType;
        }
    }
}