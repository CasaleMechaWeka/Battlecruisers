using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.UI.Cameras.InputHandlers
{
    public class MouseZoomResult
    {
        public float CameraOrthographicSize { get; private set; }
        public Vector3 CameraPosition { get; private set; }

        public MouseZoomResult(float cameraOrthographicSize, Vector3 cameraPosition)
        {
            CameraOrthographicSize = cameraOrthographicSize;
            CameraPosition = cameraPosition;
        }

        public override bool Equals(object obj)
        {
            MouseZoomResult other = obj as MouseZoomResult;
            return
                other != null
                && CameraOrthographicSize == other.CameraOrthographicSize
                && CameraPosition == other.CameraPosition;
        }

        public override int GetHashCode()
        {
            return this.GetHashCode(CameraOrthographicSize, CameraPosition);
        }
    }

	public interface IMouseZoomHandler
	{
		MouseZoomResult HandleZoom(Vector3 zoomTargetPosition, float yMouseScrollDelta);
	}
}
