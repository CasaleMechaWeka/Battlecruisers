using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Buildings.Turrets
{
	/// <summary>
	/// For turrets whose projectiles are not affected by gravity.  Ie, they fly in a straight line
	/// from the barrel tip to the enemy object.  
	/// FELIX  Lead moving targets!
	/// FELIX  Take accuracy into consideration
	/// </summary>
	public class TurretBarrelController : MonoBehaviour 
	{
		protected GameObject _targetObject;
		protected float _shellVelocityInMPerS;
		private Vector2 _targetPosition;

		private float _targetAngleInRadians;
		private Action _onRotationCompleted;

		private const float ROTATE_SPEED_IN_DEGREES_PER_S = 5;
		private const float ROTATION_EQUALITY_MARGIN_IN_RADIANS = 0.01f;

		public float DesiredAngleInRadians { get; set; }

		public event EventHandler OnTarget;

		void Update()
		{
			if (_targetObject != null)
			{
				DesiredAngleInRadians = FindDesiredAngle();
				bool isCorrectAngle = Math.Abs(transform.rotation.z - DesiredAngleInRadians) < ROTATION_EQUALITY_MARGIN_IN_RADIANS;

				Debug.Log($"TurretBarrelController.Update():  DesiredAngleInRadians: {DesiredAngleInRadians}  isCorrectAngle: {isCorrectAngle}");

				if (!isCorrectAngle)
				{
					float directionMultiplier = transform.rotation.z > DesiredAngleInRadians ? -1 : 1;
					Vector3 rotationIncrement = Vector3.forward * Time.deltaTime * ROTATE_SPEED_IN_DEGREES_PER_S * directionMultiplier;

					Debug.Log($"rotationIncrement: {rotationIncrement.ToString()}");

					transform.Rotate(rotationIncrement);
				}
				else
				{
					Debug.Log("TurretBarrelController.Update():  Have reached desired angle");
					
					// Make current rotation exactly the same as the desired rotation, to correct
					// for the ROTATION_EQUALITY_MARGIN_IN_RADIANS margin of error.
					transform.rotation = new Quaternion(
						transform.rotation.x,
						transform.rotation.y,
						DesiredAngleInRadians,
						transform.rotation.w);

					if (OnTarget != null)
					{
						OnTarget.Invoke(this, EventArgs.Empty);
					}
				}
			}
		}

		/// <summary>
		/// Assumes:
		/// 1. Shells are not affected by gravity
		/// 2. Targets do not move
		/// </summary>
		protected virtual float FindDesiredAngle()
		{
			return _targetObject.transform.rotation.z;
		}

		public void StartTrackingTarget(GameObject target, float shellVelocity)
		{
			_targetObject = target;
			_shellVelocityInMPerS = shellVelocity;
		}

		public void StopTrackingTarget()
		{
			_targetObject = null;
		}
	}

	// FELIX  Move to own file
	/// <summary>
	/// Artillery barrel wrapper controller.
  	/// FELIX  Lead moving targets!
	/// FELIX  Take accuracy into consideration
	/// </summary>
	public class ArtilleryBarrelWrapperController : TurretBarrelController
	{
		/// <summary>
		/// Assumes no y axis difference in source and target
		/// </summary>
		protected override float FindDesiredAngle()
		{
			float distanceInM = Math.Abs(transform.position.x - _targetObject.transform.position.x);
			return (float) (0.5 * Math.Asin(Constants.GRAVITY * distanceInM / (_shellVelocityInMPerS * _shellVelocityInMPerS)));
		}
	}
}
