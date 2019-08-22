using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;

namespace BattleCruisers.Scenes.Test.Offensive
{
    public class OffensiveTurretTestGod : CameraToggleTestGod
	{
        protected override void Start()
        {
            base.Start();


            Helper helper = new Helper(updaterProvider: _updaterProvider);


			// Setup target
			AirFactory target = FindObjectOfType<AirFactory>();
            helper.InitialiseBuilding(target, Faction.Blues);
			target.StartConstruction();


			// Setup turret
            TurretController turret = FindObjectOfType<TurretController>();
			ITargetFilter targetFilter = new ExactMatchTargetFilter() 
			{
				Target = target
			};
            ITargetFactories targetFactories = helper.CreateTargetFactories(target.GameObject, targetFilter: targetFilter);
            helper.InitialiseBuilding(turret, Faction.Reds, targetFactories: targetFactories);
			turret.StartConstruction();
		}
	}
}
