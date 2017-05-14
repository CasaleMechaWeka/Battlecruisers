using BattleCruisers.Buildables.Buildings.Turrets;
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

			_turretStats.fireRatePerS = 0.5f;
			_turretStats.burstFireRatePerS = 5;
			_turretStats.burstSize = 3;

			_expectedLongInterval = 1 / _turretStats.fireRatePerS;
			_expectedBurstInterval = 1 / _turretStats.burstFireRatePerS;
		}

		[Test]
		public void NextFireIntervalInS()
		{
			for (int j = 0; j < 2; ++j)
			{
				Assert.AreEqual(_expectedLongInterval, _turretStats.NextFireIntervalInS);
			
				for (int i = 0; i < _turretStats.burstSize - 1; ++i)
				{
					Assert.AreEqual(_expectedBurstInterval, _turretStats.NextFireIntervalInS);
				}
			}
		}
	}
}
