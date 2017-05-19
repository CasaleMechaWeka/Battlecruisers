using BattleCruisers.Buildables;
using BattleCruisers.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Projectiles
{
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
			Logging.Log(Tags.SHELLS, "ShellController.OnTriggerEnter2D()");

			Target target = collider.gameObject.GetComponent<Target>();

			if (target != null)
			{
				Logging.Log(Tags.SHELLS, string.Format("Own faction: {0}  Collider faction: {1}", _faction, target.Faction));
				
				if (target.Faction != _faction)
				{
					target.TakeDamage(_damage);
					Destroy(gameObject);
				}
			}
		}
	}
}