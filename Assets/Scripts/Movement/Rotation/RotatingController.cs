using System;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using UnityEngine;

namespace BattleCruisers.Movement.Rotation
{
	public class RotatingController : MonoBehaviour
	{
		private IRotationMovementController _activeRotationController, _realRotationController, _dummyRotationController;
		private float _targetAngleInDegrees;
		private bool _haveReachedDesiredAngle;

		public event EventHandler ReachedDesiredAngle;

		public void Initialise(float rotateSpeedInMPerS, float targetAngleInDegrees)
		{
			_realRotationController = new RotationMovementController(new TransformBC(transform), TimeBC.Instance, rotateSpeedInMPerS);
			_dummyRotationController = new DummyRotationMovementController(isOnTarget: false);
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
