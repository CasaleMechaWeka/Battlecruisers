using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.Turrets
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
			_turretStats.damage = 10;
			_turretStats.bulletVelocityInMPerS = 18;
			_turretStats.ignoreGravity = true;
			_turretStats.rangeInM = 20;
			_turretStats.turretRotateSpeedInDegrees = 45;

			_expectedLongInterval = 1 / _turretStats.fireRatePerS;
			_expectedBurstInterval = 1 / _turretStats.burstFireRatePerS;
		}

        /// <summary>
        /// Burst size = 3:
        /// 
        /// Duration (S = short / L = long):    S S L   S S L
        /// InBurst (T = true / F = false):     F T T   F T T
        /// </summary>
		[Test]
		public void NextFireIntervalInS_And_IsInBurst()
		{
			_turretStats.Initialise();

            // Several bursts
            for (int j = 0; j < 2; ++j)
            {
                for (int i = 0; i < _turretStats.burstSize; ++i)
                {
                    if (i == 0)
                    {
                        // First shot in burst
                        Assert.AreEqual(_expectedBurstInterval, _turretStats.DurationInS);
                        Assert.IsFalse(_turretStats.IsInBurst);
                    }
                    else if (i == _turretStats.burstSize - 1)
                    {
                        // Last shot in burst
                        Assert.AreEqual(_expectedLongInterval, _turretStats.DurationInS);
                        Assert.IsTrue(_turretStats.IsInBurst);
                    }
                    else
                    {
                        // Middle of burst
                        Assert.AreEqual(_expectedBurstInterval, _turretStats.DurationInS);
                        Assert.IsTrue(_turretStats.IsInBurst);
                    }

                    _turretStats.MoveToNextDuration();
                }
            }				
		}
	}
}
