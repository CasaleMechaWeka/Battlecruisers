using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour, IBulletController
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
