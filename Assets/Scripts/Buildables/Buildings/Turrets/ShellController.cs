using BattleCruisers.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
	// FELIX  Turn off friendly fire?
	public class ShellController : MonoBehaviour
	{
		private Faction _faction;
		private float _damage;

		public Rigidbody2D rigidBody;

		public void Initialise(Faction faction, float damage, Vector2 velocity, float gravityScale)
		{
			_faction = faction;
			_damage = damage;
			rigidBody.velocity = velocity;
			rigidBody.gravityScale = gravityScale;
		}

		void OnTriggerEnter2D(Collider2D collider)
		{
			Logging.Log(Tags.SHELLS, "BulletController.OnTriggerEnter2D()");

			IDamagable damagableObject = collider.gameObject.GetComponent<IDamagable>();
			if (damagableObject != null)
			{
				damagableObject.TakeDamage(_damage);
				Destroy(gameObject);
			}
		}
	}
}