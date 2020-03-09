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

            ISeabedImpactable targetImpactable = collider.GetComponent<ITargetProxy>()?.Target as ISeabedImpactable;

            if (targetImpactable != null)
            {
                targetImpactable.OnHitSeabed();
            }
		}
	}
}
