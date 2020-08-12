using System;
using UnityEngine;

namespace BattleCruisers.Utils.PlatformAbstractions
{
    public interface ICamera
	{
        event EventHandler OrthographicSizeChanged;
        event EventHandler PositionChanged;

		float OrthographicSize { get; set; }
        float Aspect { get; }
        float PixelWidth { get; }
        float PixelHeight { get; }
        float FieldOfView { get; set; }
        Vector3 Position { get; set; }

        Vector2 GetSize();
        Vector3 WorldToViewportPoint(Vector3 worldPoint);
        Vector3 WorldToScreenPoint(Vector3 worldPoint);
        Vector3 ScreenToWorldPoint(Vector3 screenPoint);
    }
}
