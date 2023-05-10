using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Seabed
{
    public class PvPSeabedImpacter : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D collider)
        {
            IPvPSeabedImpactable impactable = collider.GetComponent<IPvPSeabedImpactable>();

            if (impactable != null)
            {
                impactable.OnHitSeabed();
                return;
            }

            if (collider.GetComponent<IPvPTargetProxy>()?.Target is IPvPSeabedImpactable targetImpactable)
            {
                targetImpactable.OnHitSeabed();
            }
        }
    }
}
