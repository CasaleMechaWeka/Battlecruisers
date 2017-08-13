using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders.Filters;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
    public class RailgunTestGod : MonoBehaviour 
	{
		void Start()
		{
			Helper helper = new Helper();


			// Setup target
			Buildable target = FindObjectOfType<AirFactory>();
			helper.InitialiseBuildable(target, Faction.Reds);
			target.StartConstruction();


			// Setup railgun
			Buildable railgun = FindObjectOfType<TurretController>();
			ITargetFilter targetFilter = new ExactMatchTargetFilter() 
			{
				Target = target
			};
			ITargetsFactory targetsFactory = helper.CreateTargetsFactory(target.GameObject, targetFilter);
			helper.InitialiseBuildable(railgun, Faction.Blues, targetsFactory: targetsFactory);
			railgun.StartConstruction();
		}
	}
}
