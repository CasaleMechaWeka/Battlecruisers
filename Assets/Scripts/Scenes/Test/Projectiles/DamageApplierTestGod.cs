using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Projectiles.DamageAppliers;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Projectiles
{
    public class DamageApplierTestGod : MonoBehaviour 
    {
        public LayerMask targetLayerMask;

	    void Start() 
        {
            Helper helper = new Helper();
            Faction factionToTarget = Faction.Blues;


            // Setup targets
            Buildable baseTarget = FindObjectOfType<AirFactory>();
            helper.InitialiseBuildable(baseTarget, factionToTarget);

            DroneStation[] targets = FindObjectsOfType<DroneStation>();
            foreach (DroneStation target in targets)
            {
                helper.InitialiseBuildable(target, factionToTarget);
            }


            // Setup damage applier
            float damage = 50;
            float radiusInM = 5;
            ITargetFilter targetFilter = new FactionAndTargetTypeFilter(factionToTarget, TargetType.Buildings);

            IDamageApplier damageApplier = new AreaOfEffectDamageApplier(damage, radiusInM, targetFilter, targetLayerMask);
            damageApplier.ApplyDamage(baseTarget);
	    }
    }
}
