using UnityEngine;

namespace BattleCruisers.Utils.PlatformAbstractions
{
	public interface ITransform
	{
		Vector3 Position { get; set; }
        Vector3 EulerAngles { get; }
        Vector3 Right { get; }
        Vector3 Up { get; }

        void Rotate(Vector3 rotationChangeVector);
	}
}
