using System.Collections.Generic;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Utils.DataStrctures;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Tests.Aircraft
{
    public class AircraftProviderTests
	{
		private IAircraftProvider _playerAircraftProvider, _aiAircraftProvider;
		private Vector2 _playerCruiserPosition, _aiCruiserPosition;
		private float _bomberAltitude, _fighterAltitude, _deathstarAltitude;
        private BCUtils.IRandomGenerator _random;

		[SetUp]
		public void TestSetup()
		{
			_playerCruiserPosition = new Vector2(-30, 0);
			_aiCruiserPosition = new Vector2(30, 0);

			// Just return center provided, without any randomisation (ie, do not fuzz cruising altitude)
            _random = Substitute.For<BCUtils.IRandomGenerator>();
            _random.RangeFromCenter(default, default).ReturnsForAnyArgs(arg => (float)(arg.Args()[0]));

            _playerAircraftProvider = new AircraftProvider(_playerCruiserPosition, _aiCruiserPosition, _random);
            _aiAircraftProvider = new AircraftProvider(_aiCruiserPosition, _playerCruiserPosition, _random);

			_bomberAltitude = 15;
			_fighterAltitude = 20;
			_deathstarAltitude = 25;
		}

		[Test]
		public void PlayerAircraftProvider_SafeZone()
		{
			Rectangle expected 
			    = new Rectangle(
                    minX: -40, 	// -30 - 10
                    maxX: 5, 	// 30 - 25
                    minY: 10,
                    maxY: 25);

			AssertAreSafeZonesEqual(expected, _playerAircraftProvider.FighterSafeZone);
		}

		[Test]
		public void AiAircraftProvider_SafeZone()
		{
			Rectangle expected 
			    = new Rectangle(
					minX: -5, 	// -30 + 25
					maxX: 40, 	// 30 + 10
					minY: 10,
					maxY: 25);

			AssertAreSafeZonesEqual(expected, _aiAircraftProvider.FighterSafeZone);
		}

		[Test]
		public void PlayerAircraftProvider_FindBomberPatrolPoints()
		{
			IList<Vector2> patrolPoints = _playerAircraftProvider.FindBomberPatrolPoints(_bomberAltitude);

			Assert.IsTrue(patrolPoints.Contains(new Vector2(-20, _bomberAltitude - AircraftProvider.CRUISING_ALTITUDE_ERROR_MARGIN_IN_M)));  	// -30 + 10
			Assert.IsTrue(patrolPoints.Contains(new Vector2(20, _bomberAltitude - AircraftProvider.CRUISING_ALTITUDE_ERROR_MARGIN_IN_M)));		// 30 - 10
		}

		[Test]
		public void AiAircraftProvider_FindBomberPatrolPoints()
		{
			IList<Vector2> patrolPoints = _aiAircraftProvider.FindBomberPatrolPoints(_bomberAltitude);

			Assert.IsTrue(patrolPoints.Contains(new Vector2(-20, _bomberAltitude - AircraftProvider.CRUISING_ALTITUDE_ERROR_MARGIN_IN_M)));  	// -30 + 10
			Assert.IsTrue(patrolPoints.Contains(new Vector2(20, _bomberAltitude - AircraftProvider.CRUISING_ALTITUDE_ERROR_MARGIN_IN_M)));		// 30 - 10
		}

		[Test]
		public void PlayerAircraftProvider_FindFighterPatrolPoints()
		{
			IList<Vector2> patrolPoints = _playerAircraftProvider.FindFighterPatrolPoints(_fighterAltitude);

			Assert.IsTrue(patrolPoints.Contains(new Vector2(-35, _fighterAltitude)));	// -40 + 5
			Assert.IsTrue(patrolPoints.Contains(new Vector2(0, _fighterAltitude)));		// 5 - 5
		}

		[Test]
		public void AiAircraftProvider_FindFighterPatrolPoints()
		{
			IList<Vector2> patrolPoints = _aiAircraftProvider.FindFighterPatrolPoints(_fighterAltitude);

			Assert.IsTrue(patrolPoints.Contains(new Vector2(0, _fighterAltitude)));		// -5 + 5
			Assert.IsTrue(patrolPoints.Contains(new Vector2(35, _fighterAltitude)));	// 40 - 5
		}

		[Test]
		public void PlayerAircraftProvider_DeathstarPatrolPoints()
		{
			Vector2 deathstarPosition = _playerCruiserPosition;

			IList<Vector2> patrolPoints = _playerAircraftProvider.FindDeathstarPatrolPoints(deathstarPosition, _deathstarAltitude);

			Assert.IsTrue(patrolPoints.Count == 4);
			Assert.IsTrue(patrolPoints[1] == new Vector2(-30, _deathstarAltitude));
			Assert.IsTrue(patrolPoints[2] == new Vector2(35, _deathstarAltitude));	// 30 + 5
			Assert.IsTrue(patrolPoints[3] == new Vector2(25, _deathstarAltitude));	// 30 - 5
		}

		[Test]
		public void AiAircraftProvider_DeathstarPatrolPoints()
		{
			Vector2 deathstarPosition = _aiCruiserPosition;

			IList<Vector2> patrolPoints = _aiAircraftProvider.FindDeathstarPatrolPoints(deathstarPosition, _deathstarAltitude);

			Assert.IsTrue(patrolPoints.Count == 4);
			Assert.IsTrue(patrolPoints[1] == new Vector2(30, _deathstarAltitude));
			Assert.IsTrue(patrolPoints[2] == new Vector2(-35, _deathstarAltitude));	// -30 - 5
			Assert.IsTrue(patrolPoints[3] == new Vector2(-25, _deathstarAltitude));	// -30 + 5
		}

        [Test]
        public void UsesRandomisedCruisingAltitude()
        {
            _random.RangeFromCenter(default, default).ReturnsForAnyArgs(arg => (float)(arg.Args()[0]) + 1.5f);

            IList<Vector2> patrolPoints = _aiAircraftProvider.FindFighterPatrolPoints(_fighterAltitude);

            Assert.IsTrue(patrolPoints.Contains(new Vector2(0, _fighterAltitude + 1.5f)));     // -5 + 5
            Assert.IsTrue(patrolPoints.Contains(new Vector2(35, _fighterAltitude + 1.5f)));    // 40 - 5
        }

		private void AssertAreSafeZonesEqual(Rectangle expected, Rectangle actual)
		{
			Assert.AreEqual(expected.MinX, actual.MinX);
			Assert.AreEqual(expected.MaxX, actual.MaxX);
			Assert.AreEqual(expected.MinY, actual.MinY);
			Assert.AreEqual(expected.MaxY, actual.MaxY);
		}
	}
}
