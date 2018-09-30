using UnityEngine;

namespace BattleCruisers.Utils.PlatformAbstractions
{
	public interface ICamera : ITransform
	{
		float OrthographicSize { get; set; }
        float Aspect { get; }

        Vector3 WorldToViewportPoint(Vector3 worldPoint);
        Vector3 ScreenToWorldPoint(Vector3 screenPoint);
    }
}
