using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Utils;
using NSubstitute;
using System;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;

namespace BattleCruisers.Tests.Turrets
{
	public class ArtilleryAngleCalculatorTests 
	{
		private IAngleCalculator _angleCalculator;
		private ITarget _target;

		[SetUp]
		public void TestSetup()
		{
			_angleCalculator = new ArtilleryAngleCalculator(new TargetPositionPredictorFactory());

			_target = Substitute.For<ITarget>();
			_target.Position.Returns(new Vector2());
			_target.Velocity.Returns(new Vector2());
		}

		[Test]
		public void OutOfRange()
		{
			Vector2 source = new Vector2(-20, 0);
			Assert.Throws<ArgumentException>(() => _angleCalculator.FindDesiredAngle(source, _target, isSourceMirrored: false, projectileVelocityInMPerS: 2, currentAngleInRadians: 0));
		}

		[Test]
		public void SourceNotMirrored_ButTargetToLeft()
		{
			Vector2 source = new Vector2(2, 0);
			Assert.Throws<ArgumentException>(() => _angleCalculator.FindDesiredAngle(source, _target, isSourceMirrored: false, projectileVelocityInMPerS: 45, currentAngleInRadians: 0));
		}

		[Test]
		public void SourceMirrored_ButTargetToRight()
		{
			Vector2 source = new Vector2(-2, 0);
			Assert.Throws<ArgumentException>(() => _angleCalculator.FindDesiredAngle(source, _target, isSourceMirrored: true, projectileVelocityInMPerS: 45, currentAngleInRadians: 0));
		}

		[Test]
		public void MaxRange()
		{
			float velocityInMPerS = 25;
			float maxRange = (velocityInMPerS * velocityInMPerS) / Constants.GRAVITY;
			
			Vector2 source = new Vector2(maxRange, 0);
			float angleInDegrees = _angleCalculator.FindDesiredAngle(source, _target, isSourceMirrored: true, projectileVelocityInMPerS: velocityInMPerS, currentAngleInRadians: 0);

			Assert.AreEqual(45, Mathf.Round(angleInDegrees));
		}

		[Test]
		public void JustInsideMaxRange()
		{
			float velocityInMPerS = 25;
			float maxRange = (velocityInMPerS * velocityInMPerS) / Constants.GRAVITY;

			Vector2 source = new Vector2(maxRange, 0);
			float angleInDegrees = _angleCalculator.FindDesiredAngle(source, _target, isSourceMirrored: true, projectileVelocityInMPerS: velocityInMPerS + 1, currentAngleInRadians: 0);

			Assert.IsTrue(angleInDegrees < 45);
		}
	}
}
