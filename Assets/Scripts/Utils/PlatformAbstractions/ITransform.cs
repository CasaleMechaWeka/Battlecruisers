using UnityEngine;

namespace BattleCruisers.Utils.PlatformAbstractions
{
	public interface ITransform
	{
		Vector3 Position { get; set; }
        Vector3 EulerAngles { get; }

        void Rotate(Vector3 rotationChangeVector);
	}
}
