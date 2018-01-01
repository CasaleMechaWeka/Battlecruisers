using BattleCruisers.Buildables;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.Common.BuildingDetails
{
    public abstract class ComparableBuildableDetailsController<TBuildable> : BaseBuildableDetails<TBuildable>, IPointerClickHandler
        where TBuildable : class, IBuildable
	{
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
		{
			// Empty.  Simply here to eat event so parent does not receive event.
		}
	}
}
