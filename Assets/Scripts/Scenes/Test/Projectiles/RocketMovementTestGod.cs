using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Projectiles;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Projectiles.Stats.Wrappers;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
    public class RocketMovementTestGod : MonoBehaviour 
	{
		void Start()
		{
			// Setup target
			Helper helper = new Helper();
			AirFactory target = FindObjectOfType<AirFactory>();
            helper.InitialiseBuilding(target);

			
            // Setup rocket
            CruisingProjectileStats stats = GetComponent<CruisingProjectileStats>();
            ICruisingProjectileStats rocketStats = new CruisingProjectileStatsWrapper(stats);
            
            Vector2 initialVelocity = new Vector2(0, 5);

			IExactMatchTargetFilter targetFilter = new ExactMatchTargetFilter() 
			{
				Target = target
			};
			
            BuildableInitialisationArgs args = new BuildableInitialisationArgs(new Helper());

			RocketController rocket = FindObjectOfType<RocketController>();
            rocket.Initialise(rocketStats, initialVelocity, targetFilter, target, args.FactoryProvider, Faction.Blues);
		}
	}
}
