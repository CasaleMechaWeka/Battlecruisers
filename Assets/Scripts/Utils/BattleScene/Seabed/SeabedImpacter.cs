using BattleCruisers.Buildables;
using UnityEngine;

namespace BattleCruisers.Utils.BattleScene.Seabed
{
    public class SeabedImpacter : MonoBehaviour 
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
