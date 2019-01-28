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
        protected IItemDetailsManager _itemDetailsManager;
        protected IBroadcastingProperty<ItemFamily?> _itemFamilyToCompare;

        public virtual void Initialise(IItemDetailsManager itemDetailsManager, IBroadcastingProperty<ItemFamily?> itemFamilyToCompare)
        {
            Helper.AssertIsNotNull(itemDetailsManager, itemFamilyToCompare);

            _itemDetailsManager = itemDetailsManager;
            _itemFamilyToCompare = itemFamilyToCompare;
        }

        // FELIX  Avoid duplicate code in child classes?  Complicates initialisation code if this class becomes generic :/  Worth it?
        public abstract void OnPointerClick(PointerEventData eventData);
    }
}