using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Proxy;
using UnityEngine;

namespace BattleCruisers.Utils.BattleScene
{
    public class Remover : MonoBehaviour 
	{
		void OnTriggerEnter2D(Collider2D collider)
		{
            IRemovable removable = collider.GetComponent<IRemovable>();

            if (removable != null)
            {
                removable.RemoveFromScene();
                return;
            }

            IRemovable targetRemovable = collider.GetComponent<ITargetProxy>()?.Target as IRemovable;

            if (targetRemovable != null)
            {
                targetRemovable.RemoveFromScene();
            }
		}
	}
}
