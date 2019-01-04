using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.BattleScene
{
	public class Remover : MonoBehaviour 
	{
		void OnTriggerEnter2D(Collider2D collider)
		{
            IRemovable removable = collider.GetComponent<IRemovable>();
            Assert.IsNotNull(removable);
            removable.RemoveFromScene();
		}
	}
}
