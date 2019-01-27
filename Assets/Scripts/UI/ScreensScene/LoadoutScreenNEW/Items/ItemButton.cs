using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Items;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Properties;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW
{
    public abstract class ItemButton : MonoBehaviour, IPointerClickHandler
    {
        protected IItemDetailsDisplayer _itemDetailsDisplayer;
        protected IBroadcastingProperty<ItemFamily?> _itemFamilyToCompare;

        public virtual void Initialise(IItemDetailsDisplayer itemDetailsDisplayer, IBroadcastingProperty<ItemFamily?> itemFamilyToCompare)
        {
            Helper.AssertIsNotNull(itemDetailsDisplayer, itemFamilyToCompare);

            _itemDetailsDisplayer = itemDetailsDisplayer;
            _itemFamilyToCompare = itemFamilyToCompare;
        }

        // FELIX  Avoid duplicate code in child classes?  Complicates initialisation code if this class becomes generic :/  Worth it?
        public abstract void OnPointerClick(PointerEventData eventData);
    }
}