using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Tactical.Shields;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Shields
{
    public class ShieldTestsGod : TestGodBase 
	{
        protected override void Start()
        {
            base.Start();

            Helper helper = new Helper();

            // Setup shield
            ShieldGenerator shield = FindObjectOfType<ShieldGenerator>();
            Assert.IsNotNull(shield);
            helper.InitialiseBuilding(shield, Faction.Reds);
            shield.StartConstruction();

            // Setup turret
            BarrelController turret = FindObjectOfType<BarrelController>();
            turret.StaticInitialise();

            IList<TargetType> targetTypes = new List<TargetType>() { TargetType.Buildings };
            ITargetFilter targetFilter = new FactionAndTargetTypeFilter(shield.Faction, targetTypes);

            IBarrelControllerArgs barrelControllerArgs
                = helper
                    .CreateBarrelControllerArgs(
                    turret,
                    _updaterProvider.PerFrameUpdater,
                    targetFilter);

            turret.Initialise(barrelControllerArgs);
			turret.Target = shield;
		}
	}
}
