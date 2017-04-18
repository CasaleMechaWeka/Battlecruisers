using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Tactical
{
	public class ShieldController : MonoBehaviour
	{
		private Ring _ring;

		public LineRenderer lineRenderer;
		public CircleCollider2D circleCollider;

		public float shieldRadiusInM;
		public float shieldHealth;
		public float shieldRechargeDelayInS;
		public float shieldRechargeRatePerS;

		private const int NUM_OF_POINTS_IN_RING = 100;

		void Awake()
		{
			_ring = new Ring(shieldRadiusInM, NUM_OF_POINTS_IN_RING, lineRenderer);
			circleCollider.radius = shieldRadiusInM;
		}

//		public void TakeDamage(float damageAmount)
//		{
//
//		}
	}
}
