using UnityEngine;

namespace BattleCruisers
{
	public class Remover : MonoBehaviour 
	{
		void OnTriggerEnter2D(Collider2D collider)
		{
			Destroy(collider.gameObject);
		}
	}
}
