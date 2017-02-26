using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour, IBulletController
{
	public float Damage { get; set; }

	void OnTriggerEnter2D(Collider2D collider)
	{
		Debug.Log("BulletController.OnTriggerEnter2D()");

		switch (collider.tag)
		{
			case Constants.TagNames.FRIENDLY_CRUISER:
			case Constants.TagNames.ENEMY_CRUISER:
				collider.gameObject.GetComponent<ICruiser>().TakeDamage(Damage);
				Destroy(gameObject);
				break;
		}
	}
}
