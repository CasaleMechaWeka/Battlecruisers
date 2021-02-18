using BattleCruisers.Utils.PlatformAbstractions.Time;
using UnityEngine;

namespace BattleCruisers.Movement.Rotation
{
	public class ConstantRotationController : IConstantRotationController
	{
		private float _rotateSpeedInDegreesPerS;
		private Transform _transform;
        private ITime _time;

		public ConstantRotationController(float rotateSpeedInDegreesPerS, Transform transform)
		{
			_rotateSpeedInDegreesPerS = rotateSpeedInDegreesPerS;
			_transform = transform;
            _time = TimeBC.Instance;
		}

		public void Rotate()
		{
			float rotationIncrement = _time.DeltaTime * _rotateSpeedInDegreesPerS;
			Vector3 rotationIncrementVector = Vector3.forward * rotationIncrement;
			_transform.Rotate(rotationIncrementVector);
		}
	}
}