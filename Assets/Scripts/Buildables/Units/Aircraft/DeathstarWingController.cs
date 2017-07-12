using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using BattleCruisers.Movement;

namespace BattleCruisers.Buildables.Units.Aircraft
{
	public class DeathstarWingController : MonoBehaviour 
	{
		private IRotationMovementController _rotationMovementController;
		private float _desiredAngleInDegrees;

		public void Initialise(IRotationMovementController rotationMovementController)
		{
			_rotationMovementController = rotationMovementController;
			_desiredAngleInDegrees = transform.rotation.eulerAngles.z;
		}

		public void StartRotatingWing(float desiredAngleInDegrees)
		{
			_desiredAngleInDegrees = desiredAngleInDegrees;
		}

		void FixedUpdate()
		{
			if (_rotationMovementController != null
				&& !_rotationMovementController.IsOnTarget(_desiredAngleInDegrees))
			{
				_rotationMovementController.AdjustRotation(_desiredAngleInDegrees);
			}
		}
	}
}
