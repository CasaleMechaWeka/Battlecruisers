using System;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.Turrets
{
    public class AngleCalculatorTests 
	{
		private IAngleCalculator _angleCalculator;
        private Vector2 _targetPosition;

		[SetUp]
		public void TestSetup()
		{
			_angleCalculator = new AngleCalculator();
            _targetPosition = new Vector2(0, 0);
		}

		#region FindDesiredAngle
		[Test]
		public void FindDesiredAngle_SourceIsTarget_Throws()
		{
            Assert.Throws<ArgumentException>(() => _angleCalculator.FindDesiredAngle(_targetPosition, _targetPosition, isSourceMirrored: false, projectileVelocityInMPerS: -1));
		}

		#region Same axis
		[Test]
		public void FindDesiredAngle_SameX_SourceIsBelow()
		{
			Vector2 source = new Vector2(0, -2);
			TestFindDesiredAngle(source, isSourceMirrored: false, expectedAngleInDegrees: 90);
		}

		[Test]
		public void FindDesiredAngle_SameX_SourceIsBelow_Mirrored()
		{
			Vector2 source = new Vector2(0, -2);
			TestFindDesiredAngle(source, isSourceMirrored: true, expectedAngleInDegrees: 90);
		}

		[Test]
		public void FindDesiredAngle_SameX_SourceIsAbove()
		{
			Vector2 source = new Vector2(0, 2);
			TestFindDesiredAngle(source, isSourceMirrored: false, expectedAngleInDegrees: 270);
		}

		[Test]
		public void FindDesiredAngle_SameX_SourceIsAbove_Mirrored()
		{
			Vector2 source = new Vector2(0, 2);
			TestFindDesiredAngle(source, isSourceMirrored: true, expectedAngleInDegrees: 270);
		}

		[Test]
		public void FindDesiredAngle_SameY_SourceIsToLeft()
		{
			Vector2 source = new Vector2(-2, 0);
			TestFindDesiredAngle(source, isSourceMirrored: false, expectedAngleInDegrees: 0);
		}

		[Test]
		public void FindDesiredAngle_SameY_SourceIsToLeft_Mirrored()
		{
			Vector2 source = new Vector2(-2, 0);
			TestFindDesiredAngle(source, isSourceMirrored: true, expectedAngleInDegrees: 180);
		}

		[Test]
		public void FindDesiredAngle_SameY_SourceIsToRight()
		{
			Vector2 source = new Vector2(2, 0);
			TestFindDesiredAngle(source, isSourceMirrored: false, expectedAngleInDegrees: 180);
		}

		[Test]
		public void FindDesiredAngle_SameY_SourceIsToRight_Mirrored()
		{
			Vector2 source = new Vector2(2, 0);
			TestFindDesiredAngle(source, isSourceMirrored: true, expectedAngleInDegrees: 0);
		}
		#endregion Same axis

		#region Angled
		[Test]
		public void FindDesiredAngle_Source_TopLeft() 
		{
			Vector2 source = new Vector2(-2, 2);
			TestFindDesiredAngle(source, isSourceMirrored: false, expectedAngleInDegrees: 315);
		}

		[Test]
		public void FindDesiredAngle_Source_TopLeft_Mirrored() 
		{
			Vector2 source = new Vector2(-2, 2);
			TestFindDesiredAngle(source, isSourceMirrored: true, expectedAngleInDegrees: 225);
		}

		[Test]
		public void FindDesiredAngle_Source_TopRight() 
		{
			Vector2 source = new Vector2(2, 2);
			TestFindDesiredAngle(source, isSourceMirrored: false, expectedAngleInDegrees: 225);
		}

		[Test]
		public void FindDesiredAngle_Source_TopRight_Mirrored() 
		{
			Vector2 source = new Vector2(2, 2);
			TestFindDesiredAngle(source, isSourceMirrored: true, expectedAngleInDegrees: 315);
		}

		[Test]
		public void FindDesiredAngle_Source_BottomLeft() 
		{
			Vector2 source = new Vector2(-2, -2);
			TestFindDesiredAngle(source, isSourceMirrored: false, expectedAngleInDegrees: 45);
		}

		[Test]
		public void FindDesiredAngle_Source_BottomLeft_Mirrored() 
		{
			Vector2 source = new Vector2(-2, -2);
			TestFindDesiredAngle(source, isSourceMirrored: true, expectedAngleInDegrees: 135);
		}


		[Test]
		public void FindDesiredAngle_Source_BottomRight() 
		{
			Vector2 source = new Vector2(2, -2);
			TestFindDesiredAngle(source, isSourceMirrored: false, expectedAngleInDegrees: 135);
		}

		[Test]
		public void FindDesiredAngle_Source_BottomRight_Mirrored() 
		{
			Vector2 source = new Vector2(2, -2);
			TestFindDesiredAngle(source, isSourceMirrored: true, expectedAngleInDegrees: 45);
		}
		#endregion Angled

		private void TestFindDesiredAngle(Vector2 source, bool isSourceMirrored, float expectedAngleInDegrees)
		{
			float angleInDegrees = _angleCalculator.FindDesiredAngle(source, _targetPosition, isSourceMirrored: isSourceMirrored, projectileVelocityInMPerS: -1);
			Assert.AreEqual(expectedAngleInDegrees, angleInDegrees);
		}
		#endregion FindDesiredAngle
	
		#region FindDirectionMultiplier
		[Test]
		public void FindDirectionMultiplier_OnTarget()
		{
			float currentAngleInDegrees = 0;
			float targetAngleInDegrees = 0;
			 
			float directionMultiplier = _angleCalculator.FindDirectionMultiplier(currentAngleInDegrees, targetAngleInDegrees);
			Assert.AreEqual(0, directionMultiplier);
		}

		[Test]
		public void FindDirectionMultiplier_TtoS_LessThan180()
		{
			float currentAngleInDegrees = 0;
			float targetAngleInDegrees = 170;

			float directionMultiplier = _angleCalculator.FindDirectionMultiplier(currentAngleInDegrees, targetAngleInDegrees);
			Assert.AreEqual(1, directionMultiplier);
		}

		[Test]
		public void FindDirectionMultiplier_StoT_LessThan180()
		{
			float currentAngleInDegrees = 170;
			float targetAngleInDegrees = 0;

			float directionMultiplier = _angleCalculator.FindDirectionMultiplier(currentAngleInDegrees, targetAngleInDegrees);
			Assert.AreEqual(-1, directionMultiplier);
		}

		[Test]
		public void FindDirectionMultiplier_TtoS_MoreThan180()
		{
			float currentAngleInDegrees = 0;
			float targetAngleInDegrees = 190;

			float directionMultiplier = _angleCalculator.FindDirectionMultiplier(currentAngleInDegrees, targetAngleInDegrees);
			Assert.AreEqual(-1, directionMultiplier);
		}

		[Test]
		public void FindDirectionMultiplier_StoT_MoreThan180()
		{
			float currentAngleInDegrees = 190;
			float targetAngleInDegrees = 0;

			float directionMultiplier = _angleCalculator.FindDirectionMultiplier(currentAngleInDegrees, targetAngleInDegrees);
			Assert.AreEqual(1, directionMultiplier);
		}
		#endregion FindDirectionMultiplier
	}
}