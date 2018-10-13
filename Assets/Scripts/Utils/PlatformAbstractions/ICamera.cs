using UnityEngine;

namespace BattleCruisers.Utils.PlatformAbstractions
{
	public interface ICamera : ITransform
	{
		float OrthographicSize { get; set; }
        float Aspect { get; }
        float PixelWidth { get; }
        float PixelHeight { get; }

        Vector2 GetSize();
        Vector3 WorldToViewportPoint(Vector3 worldPoint);
        Vector3 WorldToScreenPoint(Vector3 worldPoint);
        Vector3 ScreenToWorldPoint(Vector3 screenPoint);
    }
}
