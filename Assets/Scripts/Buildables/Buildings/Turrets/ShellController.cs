using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Buildings.Turrets
{
	public interface IShellController
	{
		float Damage { get; set; }
	}

	public class ShellController : MonoBehaviour, IShellController
	{
		public float Damage { get; set; }

		void OnTriggerEnter2D(Collider2D collider)
		{
			Debug.Log("BulletController.OnTriggerEnter2D()");

			IDamagable damagableObject = collider.gameObject.GetComponent<IDamagable>();
			if (damagableObject != null)
			{
				damagableObject.TakeDamage(Damage);
				Destroy(gameObject);
			}
		}
	}
}