using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.ItemDetails;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW
{
    public abstract class ItemButton : MonoBehaviour, IPointerClickHandler
    {
        protected IItemDetailsDisplayer _itemDetailsDisplayer;

        public virtual void Initialise(IItemDetailsDisplayer itemDetailsDisplayer)
        {
            Assert.IsNotNull(itemDetailsDisplayer);
            _itemDetailsDisplayer = itemDetailsDisplayer;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ShowItemDetails();
        }

        protected abstract void ShowItemDetails();
    }
}