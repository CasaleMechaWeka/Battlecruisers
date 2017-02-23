using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour 
{
	void OnTriggerEnter2D(Collider2D collider)
	{
		switch (collider.tag)
		{
			case Constants.TagNames.FRIENDLY_CRUISER:
			case Constants.TagNames.ENEMY_CRUISER:
				collider.gameObject.GetComponent<Cruiser>().TakeDamage();
				Destroy(gameObject);
				break;
		}
	}
}
