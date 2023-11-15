using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene
{
    public class PvPRemover : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D collider)
        {
            IPvPRemovable removable = collider.GetComponent<IPvPRemovable>();

            if (removable != null)
            {
                removable.RemoveFromScene();
                return;
            }

            IPvPRemovable targetRemovable = collider.GetComponent<IPvPTargetProxy>()?.Target as IPvPRemovable;

            if (targetRemovable != null)
            {
                targetRemovable.RemoveFromScene();
            }
        }
    }
}
