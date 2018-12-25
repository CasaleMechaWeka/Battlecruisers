using BattleCruisers.Buildables;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.BattleScene
{
	public class Remover : MonoBehaviour 
	{
		void OnTriggerEnter2D(Collider2D collider)
		{
            IDestructable destructable = collider.GetComponent<IDestructable>();
            Assert.IsNotNull(destructable);
            destructable.Destroy();
		}
	}
}
