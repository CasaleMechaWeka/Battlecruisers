using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Tactical
{
	// FELIX  Make sure building faciton is passed on to shield
	public class ShieldController : FactionObject
	{
		private Ring _ring;

		public LineRenderer lineRenderer;
		public CircleCollider2D circleCollider;

		public float shieldRadiusInM;
		public float shieldRechargeDelayInS;
		public float shieldRechargeRatePerS;

		private const int NUM_OF_POINTS_IN_RING = 100;

		void Awake()
		{
			_ring = new Ring(shieldRadiusInM, NUM_OF_POINTS_IN_RING, lineRenderer);
			circleCollider.radius = shieldRadiusInM;
		}

		public override void TakeDamage(float damageAmount)
		{
			health -= damageAmount;

			if (health <= 0)
			{
				health = 0;
				OnDestroyed();
			}
		}
	}
}
