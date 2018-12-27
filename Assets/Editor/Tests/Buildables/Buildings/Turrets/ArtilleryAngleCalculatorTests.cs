using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils;
using NSubstitute;
using NUnit.Framework;
using System;
using UnityEngine;

namespace BattleCruisers.Tests.Buildables.Buildings.Turrets
{
    public class ArtilleryAngleCalculatorTests 
	{
		private IAngleCalculator _angleCalculator;
        private IAngleHelper _angleHelper;
        private IFlightStats _projectileFlightStats;
        private Vector2 _targetPosition;

		[SetUp]
		public void TestSetup()
		{
            _angleHelper = Substitute.For<IAngleHelper>();
            _projectileFlightStats = Substitute.For<IFlightStats>();
            _angleCalculator = new ArtilleryAngleCalculator(_angleHelper, _projectileFlightStats);

            _targetPosition = new Vector2(0, 0);
		}

		[Test]
		public void OutOfRange_Throws()
		{
			Vector2 source = new Vector2(-20, 0);
            _projectileFlightStats.MaxVelocityInMPerS.Returns(2);
			Assert.Throws<ArgumentException>(() => _angleCalculator.FindDesiredAngle(source, _targetPosition, isSourceMirrored: false));
		}

		[Test]
		public void SourceNotMirrored_ButTargetToLeft_Throws()
		{
			Vector2 source = new Vector2(2, 0);
            _projectileFlightStats.MaxVelocityInMPerS.Returns(45);
			Assert.Throws<ArgumentException>(() => _angleCalculator.FindDesiredAngle(source, _targetPosition, isSourceMirrored: false));
		}

		[Test]
		public void SourceMirrored_ButTargetToRight_Throws()
		{
			Vector2 source = new Vector2(-2, 0);
            _projectileFlightStats.MaxVelocityInMPerS.Returns(45);
			Assert.Throws<ArgumentException>(() => _angleCalculator.FindDesiredAngle(source, _targetPosition, isSourceMirrored: true));
		}

		[Test]
		public void MaxRange()
		{
			float velocityInMPerS = 25;
			float maxRange = (velocityInMPerS * velocityInMPerS) / Constants.GRAVITY;
            _projectileFlightStats.MaxVelocityInMPerS.Returns(velocityInMPerS);

            Vector2 source = new Vector2(maxRange, 0);
			float angleInDegrees = _angleCalculator.FindDesiredAngle(source, _targetPosition, isSourceMirrored: true);

			Assert.AreEqual(45, Mathf.Round(angleInDegrees));
		}

		[Test]
		public void JustInsideMaxRange()
		{
			float velocityInMPerS = 25;
			float maxRange = (velocityInMPerS * velocityInMPerS) / Constants.GRAVITY;
            _projectileFlightStats.MaxVelocityInMPerS.Returns(velocityInMPerS + 1);

            Vector2 source = new Vector2(maxRange, 0);
			float angleInDegrees = _angleCalculator.FindDesiredAngle(source, _targetPosition, isSourceMirrored: true);

			Assert.IsTrue(angleInDegrees < 45);
		}
	}
}
