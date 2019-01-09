using UnityEngine;

namespace BattleCruisers.Utils.PlatformAbstractions
{
	public interface ITransform
	{
        Transform PlatformObject { get; }

		Vector3 Position { get; set; }
        Vector3 EulerAngles { get; }
        Vector3 Right { get; }
        Vector3 Up { get; }
        Quaternion Rotation { get; }

        void Rotate(Vector3 rotationChangeVector);
	}
}
