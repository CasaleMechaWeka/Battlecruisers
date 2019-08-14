using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Projectiles;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using NSubstitute;
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
            ICruisingProjectileStats rocketStats = GetComponent<CruisingProjectileStats>();
            
            Vector2 initialVelocity = new Vector2(0, 5);

			IExactMatchTargetFilter targetFilter = new ExactMatchTargetFilter() 
			{
				Target = target
			};
			
            BuildableInitialisationArgs args = new BuildableInitialisationArgs(new Helper());

            ITarget parent = Substitute.For<ITarget>();
            parent.Faction.Returns(Faction.Blues);

			RocketController rocket = FindObjectOfType<RocketController>();
            rocket.Initialise(args.FactoryProvider);
            rocket.Activate(
                new TargetProviderActivationArgs<ICruisingProjectileStats>(
                    rocket.Position,
                    rocketStats,
                    initialVelocity,
                    targetFilter,
                    parent,
                    target));
        }
	}
}
