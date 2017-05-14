using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Projectiles;
using BattleCruisers.Utils;
using System;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;

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

			GameObject shellPrefabGameObject = new GameObject();
			_turretStats.shellPrefab = shellPrefabGameObject.AddComponent<ShellController>();

			_expectedLongInterval = 1 / _turretStats.fireRatePerS;
			_expectedBurstInterval = 1 / _turretStats.burstFireRatePerS;
		}

		[Test]
		public void NextFireIntervalInS_And_IsInBurst()
		{
			_turretStats.Initialise();

			for (int j = 0; j < 2; ++j)
			{
				Assert.AreEqual(_expectedLongInterval, _turretStats.NextFireIntervalInS);
				Assert.IsFalse(_turretStats.IsInBurst);

				for (int i = 0; i < _turretStats.burstSize - 1; ++i)
				{
					Assert.AreEqual(_expectedBurstInterval, _turretStats.NextFireIntervalInS);
					Assert.IsTrue(_turretStats.IsInBurst);
				}
			}
		}
	}
}
