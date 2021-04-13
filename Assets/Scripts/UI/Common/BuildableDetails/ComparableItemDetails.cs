using BattleCruisers.Buildables;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public abstract class ComparableItemDetails<TItem> : ItemDetails<TItem>, IPointerClickHandler
        where TItem : class, ITarget, IComparableItem
	{
		public GameObject rightSide;

        public override void Initialise()
        {
            base.Initialise();
            Assert.IsNotNull(rightSide);
        }

        public override void ShowItemDetails(TItem item, TItem itemToCompareTo = null)
        {
            base.ShowItemDetails(item, itemToCompareTo);

            // There is only space for the right side if there is no comparison item
            rightSide.SetActive(itemToCompareTo == null);
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
		{
			// Empty.  Simply here to eat event so parent does not receive event.
		}
	}
}
