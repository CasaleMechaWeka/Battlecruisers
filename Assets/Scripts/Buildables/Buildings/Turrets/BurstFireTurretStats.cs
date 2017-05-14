using BattleCruisers.Projectiles;
using BattleCruisers.Utils;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
	public class BurstFireTurretStats : TurretStats
	{
		private int _intervalCounter;

		public int burstSize;
		public float burstFireRatePerS;

		private const int MIN_BURST_SIZE = 3;

		public override float DamagePerS
		{
			get
			{
				float cycleDamage = burstSize * damage;
				float cycleTime = (1 / fireRatePerS) + burstSize * (1 / burstFireRatePerS);
				return cycleDamage / cycleTime;
			}
		}

		public override float NextFireIntervalInS
		{
			get
			{
				float nextFireIntervalInS;
				
				if (++_intervalCounter == burstSize)
				{
					_intervalCounter = 0;
				}

				if (IsInBurst)
				{
					nextFireIntervalInS = 1 / burstFireRatePerS;
				}
				else
				{
					nextFireIntervalInS = 1 / fireRatePerS;
				}

				return nextFireIntervalInS;
			}
		}

		public override bool IsInBurst
		{
			get
			{
				return _intervalCounter > 0 && _intervalCounter <= burstSize;
			}
		}

		public override void Initialise()
		{
			base.Initialise();

			Assert.IsTrue(burstSize >= MIN_BURST_SIZE);
			Assert.IsTrue(burstFireRatePerS > 0);

			_intervalCounter = -1;
		}
	}
}