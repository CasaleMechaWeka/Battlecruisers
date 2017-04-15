using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;

namespace BattleCruisers.Tests.Turrets
{
	public class AngleCalculatorTests 
	{
		private AngleCalculator _angleCalculator;
		private Vector2 _target;

		[SetUp]
		public void TestSetup()
		{
			GameObject gameObject = new GameObject();
			_angleCalculator = gameObject.AddComponent<AngleCalculator>();
			_target = new Vector2();

			Logging.Initialise();
		}

		[Test]
		public void Source_TopLeft() 
		{
			Vector2 source = new Vector2(-2, 2);
			TestFindDesiredAngle(source, isSourceMirrored: false, expectedAngleInDegrees: 315);
		}

		[Test]
		public void Source_TopLeft_Mirrored() 
		{
			Vector2 source = new Vector2(-2, 2);
			TestFindDesiredAngle(source, isSourceMirrored: true, expectedAngleInDegrees: 225);
		}

		[Test]
		public void Source_TopRight() 
		{
			Vector2 source = new Vector2(2, 2);
			TestFindDesiredAngle(source, isSourceMirrored: false, expectedAngleInDegrees: 225);
		}

		[Test]
		public void Source_TopRight_Mirrored() 
		{
			Vector2 source = new Vector2(2, 2);
			TestFindDesiredAngle(source, isSourceMirrored: true, expectedAngleInDegrees: 315);
		}

		[Test]
		public void Source_BottomLeft() 
		{
			Vector2 source = new Vector2(-2, -2);
			TestFindDesiredAngle(source, isSourceMirrored: false, expectedAngleInDegrees: 45);
		}

		[Test]
		public void Source_BottomLeft_Mirrored() 
		{
			Vector2 source = new Vector2(-2, -2);
			TestFindDesiredAngle(source, isSourceMirrored: true, expectedAngleInDegrees: 135);
		}


		[Test]
		public void Source_BottomRight() 
		{
			Vector2 source = new Vector2(2, -2);
			TestFindDesiredAngle(source, isSourceMirrored: false, expectedAngleInDegrees: 135);
		}

		[Test]
		public void Source_BottomRight_Mirrored() 
		{
			Vector2 source = new Vector2(2, -2);
			TestFindDesiredAngle(source, isSourceMirrored: true, expectedAngleInDegrees: 45);
		}

		private void TestFindDesiredAngle(Vector2 source, bool isSourceMirrored, float expectedAngleInDegrees)
		{
			float angleInDegrees = _angleCalculator.FindDesiredAngle(source, _target, isSourceMirrored: isSourceMirrored, projectileVelocityInMPerS: -1);
			Assert.AreEqual(expectedAngleInDegrees, angleInDegrees);
		}
	}
}