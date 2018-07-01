using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.Buildables.Buildings.Turrets.Stats
{
    public class BurstFireTurretStatsTests 
	{
		private BurstFireTurretStats _turretStats;

		private float _expectedLongInterval;
		private float _expectedBurstInterval;

		[SetUp]
		public void TestSetup()
		{
			GameObject gameObject = new GameObject();
			_turretStats = gameObject.AddComponent<BurstFireTurretStats>();

			// Fields required for properites we are testing
			_turretStats.fireRatePerS = 0.5f;
			_turretStats.burstFireRatePerS = 5;
			_turretStats.burstSize = 3;

			// Additional fields required to stop assets from failing
			_turretStats.accuracy = 1;
			_turretStats.rangeInM = 20;
			_turretStats.turretRotateSpeedInDegrees = 45;

			_expectedLongInterval = 1 / _turretStats.fireRatePerS;
			_expectedBurstInterval = 1 / _turretStats.burstFireRatePerS;

            _turretStats.attackCapabilities = new List<TargetType>()
            {
                TargetType.Aircraft
            };

            _turretStats.Initialise();
		}

        /// <summary>
        /// Burst size = 3:
        /// 
        /// InBurst (T = true / F = false):         F  T  T    F  T  T
        /// Turret fires (MoveToNextDuration()):     *  *  *    *  *  *
        /// Duration (S = short / L = long):          S  S  L    S  S  L
        /// </summary>
        [Test]
		public void NextFireIntervalInS_And_IsInBurst()
		{
            int numOfBursts = 10;

            bool[] expectedIsInBursts = new bool[] { false, true, true };
            float[] expectedDurations = new float[] { _expectedBurstInterval, _expectedBurstInterval, _expectedLongInterval };

            for (int j = 0; j < numOfBursts; ++j)
            {
                for (int i = 0; i < _turretStats.burstSize; ++i)
                {
                    Assert.AreEqual(expectedIsInBursts[i], _turretStats.IsInBurst);

                    // Turret fires
                    _turretStats.MoveToNextDuration();

                    Assert.AreEqual(expectedDurations[i], _turretStats.DurationInS);
                }
            }				
		}
	}
}
