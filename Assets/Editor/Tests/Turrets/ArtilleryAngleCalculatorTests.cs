using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Utils;
using System;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;

namespace BattleCruisers.Tests.Turrets
{
	public class ArtilleryAngleCalculatorTests 
	{
		private IAngleCalculator _angleCalculator;
		private Vector2 _target;

		[SetUp]
		public void TestSetup()
		{
			GameObject gameObject = new GameObject();
			_angleCalculator = gameObject.AddComponent<ArtilleryAngleCalculator>();
			_target = new Vector2();

			Logging.Initialise();
		}

		[ExpectedException(typeof(ArgumentException))]
		[Test]
		public void OutOfRange()
		{
			Vector2 source = new Vector2(-20, 0);
			_angleCalculator.FindDesiredAngle(source, _target, isSourceMirrored: false, projectileVelocityInMPerS: 2);
		}

		[ExpectedException(typeof(ArgumentException))]
		[Test]
		public void SourceNotMirrored_ButTargetToLeft()
		{
			Vector2 source = new Vector2(2, 0);
			_angleCalculator.FindDesiredAngle(source, _target, isSourceMirrored: false, projectileVelocityInMPerS: 45);
		}

		[ExpectedException(typeof(ArgumentException))]
		[Test]
		public void SourceMirrored_ButTargetToRight()
		{
			Vector2 source = new Vector2(-2, 0);
			_angleCalculator.FindDesiredAngle(source, _target, isSourceMirrored: true, projectileVelocityInMPerS: 45);
		}

		[Test]
		public void MaxRange()
		{
			float velocityInMPerS = 25;
			float maxRange = (velocityInMPerS * velocityInMPerS) / Constants.GRAVITY;
			
			Vector2 source = new Vector2(maxRange, 0);
			float angleInDegrees = _angleCalculator.FindDesiredAngle(source, _target, isSourceMirrored: true, projectileVelocityInMPerS: velocityInMPerS);

			Assert.AreEqual(45, Mathf.Round(angleInDegrees));
		}

		[Test]
		public void JustInsideMaxRange()
		{
			float velocityInMPerS = 25;
			float maxRange = (velocityInMPerS * velocityInMPerS) / Constants.GRAVITY;

			Vector2 source = new Vector2(maxRange, 0);
			float angleInDegrees = _angleCalculator.FindDesiredAngle(source, _target, isSourceMirrored: true, projectileVelocityInMPerS: velocityInMPerS + 1);

			Assert.IsTrue(angleInDegrees < 45);
		}
	}
}
