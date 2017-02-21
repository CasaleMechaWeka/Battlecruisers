using UnityEngine;

public class Remover : MonoBehaviour 
{
	void OnTriggerEnter2D(Collider2D collider)
	{
		Destroy(collider.gameObject);
	}
}
