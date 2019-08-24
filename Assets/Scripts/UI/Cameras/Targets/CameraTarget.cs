using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.UI.Cameras.Targets
{
    // PERF  struct candidate :)
    public class CameraTarget : ICameraTarget
    {
		public Vector3 Position { get; } 
		public float OrthographicSize { get; }

		public CameraTarget(Vector3 position, float orthographicSize)
		{
			Position = position;
			OrthographicSize = orthographicSize;
		}

        public override bool Equals(object obj)
        {
            CameraTarget other = obj as CameraTarget;
            return
                other != null
                && Position == other.Position
                && Mathf.Approximately(OrthographicSize, other.OrthographicSize);
        }

        public override int GetHashCode()
        {
            return this.GetHashCode(Position, OrthographicSize);
        }

        public override string ToString()
        {
            return "CameraTarget: " + Position + "  Orthographic size: " + OrthographicSize;
        }
    }
}
