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
		private float _maxHealth;

		public LineRenderer lineRenderer;
		public CircleCollider2D circleCollider;

		public float shieldRadiusInM;
		public float shieldRechargeDelayInS;
		public float shieldRechargeRatePerS;

		private const int NUM_OF_POINTS_IN_RING = 100;

		void Awake()
		{
			_ring = new Ring(shieldRadiusInM, NUM_OF_POINTS_IN_RING, lineRenderer);
			_timeSinceDamageInS = 0;
			_maxHealth = health;
			circleCollider.radius = shieldRadiusInM;
		}

		void Update()
		{
			// Eat into recharge delay
			if (health < _maxHealth)
			{
				_timeSinceDamageInS += Time.deltaTime;
			}

			// Heal
			if (_timeSinceDamageInS >= shieldRechargeDelayInS)
			{
				if (IsDestroyed)
				{
					EnableShield();
				}

				health += shieldRechargeRatePerS * Time.deltaTime;

				if (health > _maxHealth)
				{
					health = _maxHealth;
				}
			}
		}

		public override void TakeDamage(float damageAmount)
		{
			health -= damageAmount;

			_timeSinceDamageInS = 0;

			if (health <= 0)
			{
				health = 0;

				DisableShield();

				OnDestroyed();
			}
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
