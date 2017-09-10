using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders.Filters;

namespace BattleCruisers.Scenes.Test.Offensive
{
    public class RocketLauncherTestGod : CameraToggleTestGod
	{
		protected override void OnStart()
		{
			Helper helper = new Helper();


			// Setup target
			AirFactory target = FindObjectOfType<AirFactory>();
            helper.InitialiseBuilding(target, Faction.Blues);
			target.StartConstruction();


			// Setup rocket launcher
			TurretController rocketLauncher = FindObjectOfType<TurretController>();
			ITargetFilter targetFilter = new ExactMatchTargetFilter() 
			{
				Target = target
			};
			ITargetsFactory targetsFactory = helper.CreateTargetsFactory(target.GameObject, targetFilter);
            helper.InitialiseBuilding(rocketLauncher, Faction.Reds, targetsFactory: targetsFactory);
			rocketLauncher.StartConstruction();
		}
	}
}
