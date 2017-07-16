using UnityEngine;

namespace BattleCruisers.Movement.Rotation
{
	public class ConstantRotationController : IConstantRotationController
	{
		private float _rotateSpeedInDegreesPerS;
		private Transform _transform;

		public ConstantRotationController(float rotateSpeedInDegreesPerS, Transform transform)
		{
			_rotateSpeedInDegreesPerS = rotateSpeedInDegreesPerS;
			_transform = transform;
		}

		public void Rotate()
		{
			Debug.Log("Rotate()");

			float rotationIncrement = Time.deltaTime * _rotateSpeedInDegreesPerS;
			Vector3 rotationIncrementVector = Vector3.forward * rotationIncrement;
			_transform.Rotate(rotationIncrementVector);
		}
	}
}