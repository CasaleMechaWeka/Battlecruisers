using UnityEngine;

namespace BattleCruisers.Utils.BattleScene
{
	public class Remover : MonoBehaviour 
	{
		void OnTriggerEnter2D(Collider2D collider)
		{
			Destroy(collider.gameObject);
		}
	}
}
