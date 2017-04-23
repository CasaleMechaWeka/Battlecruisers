using BattleCruisers.UI;
using BattleCruisers.UI.ProgressBars;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Tactical
{
	// FELIX  Make sure building faction is passed on to shield
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
		private const float HEALTH_BAR_Y_POSITION_MULTIPLIER = 1.2f;
		private const float SHIELD_RADIUS_TO_HEALTH_BAR_WIDTH_MULTIPLIER = 1.6f;
		private const float HEALTH_BAR_WIDTH_TO_HEIGHT_MULTIPLIER = 0.025f;

		public void Initialise(Faction faction)
		{
			Faction = faction;

			_ring = new Ring(shieldRadiusInM, NUM_OF_POINTS_IN_RING, lineRenderer);
			_timeSinceDamageInS = 0;
			circleCollider.radius = shieldRadiusInM;

			SetupHealthBar();
		}

		private void SetupHealthBar()
		{
			healthBar.Initialise(this);
			
			float yPos = HEALTH_BAR_Y_POSITION_MULTIPLIER * shieldRadiusInM;
			healthBar.UpdatePosition(new Vector2(transform.position.x, transform.position.y + yPos));
			
			float width = SHIELD_RADIUS_TO_HEALTH_BAR_WIDTH_MULTIPLIER * shieldRadiusInM;
			float height = HEALTH_BAR_WIDTH_TO_HEIGHT_MULTIPLIER * width;
			healthBar.UpdateSize(width, height);
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
