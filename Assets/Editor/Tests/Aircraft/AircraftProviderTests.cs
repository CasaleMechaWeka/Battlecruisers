using BattleCruisers.Utils;
using BattleCruisers.Units.Aircraft.Providers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BattleCruisers.Tests.Aircraft
{
	public class AircraftProviderTests
	{
		private IAircraftProvider _aircraftProvider;
		private Vector2 _parentCruiserPosition, _enemyCruiserPosition;
		private float _bomberAltitude, _fighterAltitude;

		[SetUp]
		public void TestSetup()
		{
			_parentCruiserPosition = new Vector2(-30, 0);
			_enemyCruiserPosition = new Vector2(30, 0);

			_aircraftProvider = new AircraftProvider(_parentCruiserPosition, _enemyCruiserPosition);

			_bomberAltitude = 15;
			_fighterAltitude = 20;
		}

		[Test]
		public void SafeZone()
		{
			SafeZone expectedSafeZone 
				= new SafeZone(
                    minX: -40, 	// -30 - 10
                    maxX: 5, 	// 30 - 25
                    minY: 10,
                    maxY: 25);
			
			Assert.AreEqual(expectedSafeZone.MinX, _aircraftProvider.FighterSafeZone.MinX);
			Assert.AreEqual(expectedSafeZone.MaxX, _aircraftProvider.FighterSafeZone.MaxX);
			Assert.AreEqual(expectedSafeZone.MinY, _aircraftProvider.FighterSafeZone.MinY);
			Assert.AreEqual(expectedSafeZone.MaxY, _aircraftProvider.FighterSafeZone.MaxY);
		}

		[Test]
		public void FindBomberPatrolPoints()
		{
			IList<Vector2> patrolPoints = _aircraftProvider.FindBomberPatrolPoints(_bomberAltitude);

			Assert.IsTrue(patrolPoints.Contains(new Vector2(-20, _bomberAltitude)));  	// -30 + 10
			Assert.IsTrue(patrolPoints.Contains(new Vector2(20, _bomberAltitude)));		// 30 - 10
		}

		[Test]
		public void FindFighterPatrolPoints()
		{
			IList<Vector2> patrolPoints = _aircraftProvider.FindFighterPatrolPoints(_fighterAltitude);

			Assert.IsTrue(patrolPoints.Contains(new Vector2(-35, _fighterAltitude)));	// -40 + 5
			Assert.IsTrue(patrolPoints.Contains(new Vector2(0, _fighterAltitude)));	// 5 - 5
		}
	}
}
