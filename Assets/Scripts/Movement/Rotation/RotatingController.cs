using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Rotation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Movement.Rotation
{
	public class RotatingController : MonoBehaviour 
	{
		private IRotationMovementController _activeRotationController, _realRotationController, _dummyRotationController;
		private float _targetAngleInDegrees;
		private bool _haveReachedDesiredAngle;

		public event EventHandler ReachedDesiredAngle;

		public void Initialise(IMovementControllerFactory movementControllerFactory, float rotateSpeedInMPerS, float targetAngleInDegrees)
		{
			_realRotationController = movementControllerFactory.CreateRotationMovementController(rotateSpeedInMPerS, transform);
			_dummyRotationController = movementControllerFactory.CreateDummyRotationMovementController(isOnTarget: false);
			_activeRotationController = _dummyRotationController;

			_targetAngleInDegrees = targetAngleInDegrees;
			_haveReachedDesiredAngle = false;
		}

		public void StartRotating()
		{
			_activeRotationController = _realRotationController;
		}

		void FixedUpdate()
		{
			if (!_activeRotationController.IsOnTarget(_targetAngleInDegrees))
			{
				_activeRotationController.AdjustRotation(_targetAngleInDegrees);
			}
			else
			{
				if (!_haveReachedDesiredAngle)
				{
					_haveReachedDesiredAngle = true;

					ReachedDesiredAngle?.Invoke(this, EventArgs.Empty);
				}
			}
		}
	}
}
