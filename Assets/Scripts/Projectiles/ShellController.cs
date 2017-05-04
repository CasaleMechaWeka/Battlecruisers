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

			Target factionObject = collider.gameObject.GetComponent<Target>();

			if (factionObject != null)
			{
				Logging.Log(Tags.SHELLS, $"Own faction: {_faction}  Collider faction: {factionObject.Faction}");
				
				if (factionObject.Faction != _faction)
				{
					factionObject.TakeDamage(_damage);
					Destroy(gameObject);
				}
			}
		}
	}
}