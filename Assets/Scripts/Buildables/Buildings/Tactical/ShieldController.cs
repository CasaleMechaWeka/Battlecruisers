using BattleCruisers.UI;
using BattleCruisers.UI.ProgressBars;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Tactical
{
	// FELIX  Make sure building faciton is passed on to shield
	public class ShieldController : FactionObject
	{
		private Ring _ring;
		private float _timeSinceDamageInS;

		public LineRenderer lineRenderer;
		public CircleCollider2D circleCollider;
		public HealthBarController healthBar;

		public float shieldRadiusInM;
		public float shieldRechargeDelayInS;
		public float shieldRechargeRatePerS;

		private const int NUM_OF_POINTS_IN_RING = 100;

		protected override void OnAwake()
		{
			_ring = new Ring(shieldRadiusInM, NUM_OF_POINTS_IN_RING, lineRenderer);
			_timeSinceDamageInS = 0;
			circleCollider.radius = shieldRadiusInM;
		}

		void Update()
		{
			// Eat into recharge delay
			if (Health < maxHealth)
			{
				_timeSinceDamageInS += Time.deltaTime;

				// Heal
				if (_timeSinceDamageInS >= shieldRechargeDelayInS)
				{
					if (IsDestroyed)
					{
						EnableShield();
					}

					Repair(shieldRechargeRatePerS * Time.deltaTime);
				}
			}
		}

		protected override void OnDestroyed()
		{
			DisableShield();
		}

		protected override void OnTakeDamage()
		{
			_timeSinceDamageInS = 0;
		}

		private void EnableShield()
		{
			_ring.Enabled = true;
			circleCollider.enabled = true;
		}

		private void DisableShield()
		{
			_ring.Enabled = false;
			circleCollider.enabled = false;
		}
	}
}
