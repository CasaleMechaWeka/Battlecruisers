using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Projectiles.DamageAppliers;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Projectiles
{
    public class DamageApplierTestGod : MonoBehaviour 
    {
	    void Start() 
        {
            Helper helper = new Helper();
            Faction factionToTarget = Faction.Blues;


            // Setup targets
            IBuildable baseTarget = FindObjectOfType<AirFactory>();
			
			TestAircraftController aircraft = FindObjectOfType<TestAircraftController>();
			aircraft.UseDummyMovementController = true;
            helper.InitialiseUnit(aircraft, factionToTarget);

            IBuilding[] targets = FindObjectsOfType<Building>();
            foreach (IBuilding target in targets)
            {
                helper.InitialiseBuilding(target, factionToTarget);
            }


            // Setup damage applier
            IDamageStats damageStats = new DamageStats(damage: 50, damageRadiusInM: 5);
            ITargetFilter targetFilter = new DummyTargetFilter(isMatchResult: true);
            IDamageApplier damageApplier = new AreaOfEffectDamageApplier(damageStats, targetFilter);

            damageApplier.ApplyDamage(baseTarget, baseTarget.Position, damageSource: null);
	    }
    }
}
