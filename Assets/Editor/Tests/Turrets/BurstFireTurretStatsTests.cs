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

		[Test]
		public void NextFireIntervalInS_And_IsInBurst()
		{
			_turretStats.Initialise();

			for (int j = 0; j < 2; ++j)
			{
                // Short interval in burst
                for (int i = 0; i < _turretStats.burstSize - 1; ++i)
                {
                    Assert.AreEqual(_expectedBurstInterval, _turretStats.DurationInS);
                    if (i != 0)
                    {
                        Assert.IsTrue(_turretStats.IsInBurst);
					}
                    _turretStats.MoveToNextDuration();
                }
				
                // Long interval in between bursts
                Assert.AreEqual(_expectedLongInterval, _turretStats.DurationInS);
				Assert.IsFalse(_turretStats.IsInBurst);
				_turretStats.MoveToNextDuration();
			}
		}
	}
}
