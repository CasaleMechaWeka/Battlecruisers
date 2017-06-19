using System;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.Common.BuildingDetails
{
	public class ComparableBuildableDetailsController : BaseBuildableDetails, IPointerClickHandler
	{
		void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
		{
			// Empty.  Simply here to eat event so parent does not receive event.
		}
	}
}
