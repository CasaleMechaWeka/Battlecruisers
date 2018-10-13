using UnityEngine;

namespace BattleCruisers.Utils.PlatformAbstractions
{
	public interface ICamera : ITransform
	{
		float OrthographicSize { get; set; }
        float Aspect { get; }
        Vector2 Size { get; }

        Vector3 WorldToViewportPoint(Vector3 worldPoint);
        Vector3 WorldToScreenPoint(Vector3 worldPoint);
        Vector3 ScreenToWorldPoint(Vector3 screenPoint);
    }
}
