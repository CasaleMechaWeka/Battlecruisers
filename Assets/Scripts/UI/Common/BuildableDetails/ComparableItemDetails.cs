using BattleCruisers.Buildables;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Comparisons;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public abstract class ComparableItemDetails<TItem> : ItemDetails<TItem>, IPointerClickHandler
        where TItem : class, ITarget, IComparableItem
	{
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
		{
			// Empty.  Simply here to eat event so parent does not receive event.
		}
	}
}
