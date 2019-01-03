using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.Factories;
using BattleCruisers.Targets.TargetFinders.Filters;

namespace BattleCruisers.Scenes.Test.Offensive
{
    public class OffensiveTurretTestGod : CameraToggleTestGod
	{
		protected override void OnStart()
		{
			Helper helper = new Helper();


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
            ITargetFactoriesProvider targetFactories = helper.CreateTargetFactories(target.GameObject, targetFilter);
            helper.InitialiseBuilding(turret, Faction.Reds, targetFactories: targetFactories);
			turret.StartConstruction();
		}
	}
}
