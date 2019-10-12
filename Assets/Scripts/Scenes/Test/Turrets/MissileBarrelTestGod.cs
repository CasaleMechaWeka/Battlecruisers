using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using System.Collections.Generic;

namespace BattleCruisers.Scenes.Test.Turrets
{
    public class MissileBarrelTestGod : TestGodBase 
    {
	    protected override void Start() 
        {
            base.Start();


            Helper helper = new Helper();


            // Setup target
            AirFactory target = FindObjectOfType<AirFactory>();
            helper.InitialiseBuilding(target, Faction.Reds);
            target.StartConstruction();


            // Setup missile barrel controller
            IList<TargetType> targetTypes = new List<TargetType>() { TargetType.Buildings };
            ITargetFilter targetFilter = new FactionAndTargetTypeFilter(Faction.Reds, targetTypes);

            MissileBarrelController missileBarrel = FindObjectOfType<MissileBarrelController>();
            missileBarrel.StaticInitialise();
			missileBarrel.Target = target;

            IBarrelControllerArgs barrelControllerArgs
                = helper.CreateBarrelControllerArgs(
                    missileBarrel, 
                    _updaterProvider.PerFrameUpdater,
                    targetFilter: targetFilter,
                    angleCalculator: new StaticAngleCalculator(new AngleHelper(), desiredAngleInDegrees: 90));

            missileBarrel.InitialiseAsync(barrelControllerArgs);
	    }
	}
}
