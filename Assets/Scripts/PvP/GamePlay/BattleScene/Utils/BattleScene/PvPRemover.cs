using BattleCruisers.Buildables;
using BattleCruisers.Utils.BattleScene;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene
{
    public class PvPRemover : MonoBehaviour
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
