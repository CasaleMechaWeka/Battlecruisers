using BattleCruisers.Buildables;
using BattleCruisers.Utils.BattleScene.Seabed;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Seabed
{
    public class PvPSeabedImpacter : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D collider)
        {
            ISeabedImpactable impactable = collider.GetComponent<ISeabedImpactable>();

            if (impactable != null)
            {
                impactable.OnHitSeabed();
                return;
            }

            if (collider.GetComponent<ITargetProxy>()?.Target is ISeabedImpactable targetImpactable)
            {
                targetImpactable.OnHitSeabed();
            }
        }
    }
}
