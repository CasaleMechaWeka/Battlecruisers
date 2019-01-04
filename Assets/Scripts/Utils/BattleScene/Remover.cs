using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.BattleScene
{
	public class Remover : MonoBehaviour 
	{
		void OnTriggerEnter2D(Collider2D collider)
		{
            IDestructable removable = collider.GetComponent<IDestructable>();
            Assert.IsNotNull(removable);
            removable.RemoveFromScene();
		}
	}
}
