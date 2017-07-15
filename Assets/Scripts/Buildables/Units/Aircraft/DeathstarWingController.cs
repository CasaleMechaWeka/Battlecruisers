using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Movement.Rotation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units.Aircraft
{
	public class DeathstarWingController : MonoBehaviour 
	{
		private IRotationMovementController _rotationMovementController;
		private bool _haveReachedDesiredAngle;

		private float _desiredAngleInDegrees;
		private float DesiredAngleInDegrees
		{
			get { return _desiredAngleInDegrees; }
			set
			{
				_desiredAngleInDegrees = value;
				_haveReachedDesiredAngle = false;
			}
		}

		public event EventHandler ReachedDesiredAngle;

		public void Initialise(IRotationMovementController rotationMovementController)
		{
			_rotationMovementController = rotationMovementController;
			DesiredAngleInDegrees = transform.rotation.eulerAngles.z;
		}

		public void StartRotatingWing(float desiredAngleInDegrees)
		{
			DesiredAngleInDegrees = desiredAngleInDegrees;
		}

		// FELIX  Shouldn't need null check :/
		void FixedUpdate()
		{
			if (_rotationMovementController != null)
			{
				if (!_rotationMovementController.IsOnTarget(_desiredAngleInDegrees))
				{
					_rotationMovementController.AdjustRotation(_desiredAngleInDegrees);
				}
				else
				{
					if (!_haveReachedDesiredAngle)
					{
						_haveReachedDesiredAngle = true;
						if (ReachedDesiredAngle != null)
						{
							ReachedDesiredAngle.Invoke(this, EventArgs.Empty);
						}
					}
				}
			}
		}
	}
}
