using System;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Utils;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.Buildables.Buildings.Turrets
{
    public class ArtilleryAngleCalculatorTests 
	{
		private IAngleCalculator _angleCalculator;
        private IAngleHelper _angleHelper;
        private Vector2 _targetPosition;

		[SetUp]
		public void TestSetup()
		{
            _angleHelper = Substitute.For<IAngleHelper>();
            _angleCalculator = new ArtilleryAngleCalculator(_angleHelper);
            _targetPosition = new Vector2(0, 0);
		}

		[Test]
		public void OutOfRange_Throws()
		{
			Vector2 source = new Vector2(-20, 0);
			Assert.Throws<ArgumentException>(() => _angleCalculator.FindDesiredAngle(source, _targetPosition, isSourceMirrored: false, projectileVelocityInMPerS: 2));
		}

		[Test]
		public void SourceNotMirrored_ButTargetToLeft_Throws()
		{
			Vector2 source = new Vector2(2, 0);
			Assert.Throws<ArgumentException>(() => _angleCalculator.FindDesiredAngle(source, _targetPosition, isSourceMirrored: false, projectileVelocityInMPerS: 45));
		}

		[Test]
		public void SourceMirrored_ButTargetToRight_Throws()
		{
			Vector2 source = new Vector2(-2, 0);
			Assert.Throws<ArgumentException>(() => _angleCalculator.FindDesiredAngle(source, _targetPosition, isSourceMirrored: true, projectileVelocityInMPerS: 45));
		}

		[Test]
		public void MaxRange()
		{
			float velocityInMPerS = 25;
			float maxRange = (velocityInMPerS * velocityInMPerS) / Constants.GRAVITY;
			
			Vector2 source = new Vector2(maxRange, 0);
			float angleInDegrees = _angleCalculator.FindDesiredAngle(source, _targetPosition, isSourceMirrored: true, projectileVelocityInMPerS: velocityInMPerS);

			Assert.AreEqual(45, Mathf.Round(angleInDegrees));
		}

		[Test]
		public void JustInsideMaxRange()
		{
			float velocityInMPerS = 25;
			float maxRange = (velocityInMPerS * velocityInMPerS) / Constants.GRAVITY;

			Vector2 source = new Vector2(maxRange, 0);
			float angleInDegrees = _angleCalculator.FindDesiredAngle(source, _targetPosition, isSourceMirrored: true, projectileVelocityInMPerS: velocityInMPerS + 1);

			Assert.IsTrue(angleInDegrees < 45);
		}
	}
}
