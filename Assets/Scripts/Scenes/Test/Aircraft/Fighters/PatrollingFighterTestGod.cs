using System.Collections.Generic;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Scenes.Test.Utilities;
using NSubstitute;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Aircraft.Fighters
{
    /// <summary>
    /// Created because I had an issue where the fighter's patrolling behaviour
    /// was stuffed :P  Hence have test scene specifically for checking this.
    /// </summary>
    public class PatrollingFighterTestGod : TestGodBase
	{
        private FighterController _fighter;

        protected override List<GameObject> GetGameObjects()
        {
            _fighter = FindObjectOfType<FighterController>();
            return new List<GameObject>()
            {
                _fighter.GameObject
            };
        }

        protected override void Setup(Helper helper)
        {
            IAircraftProvider provider = CreateAircraftProvider();
            helper.InitialiseUnit(_fighter, aircraftProvider: provider);
            _fighter.StartConstruction();
		}

        private IAircraftProvider CreateAircraftProvider()
        {
            IAircraftProvider provider = Substitute.For<IAircraftProvider>();

            IList<Vector2> fighterPatrolPoints = new List<Vector2>()
            {
                new Vector2(-2, 5),
                new Vector2(2, 5),
                new Vector2(5, 2),
                new Vector2(5, -2),
                new Vector2(2, -5),
                new Vector2(-2, -5),
                new Vector2(-5, -2),
                new Vector2(-5, 2)
            };

            provider.FindFighterPatrolPoints(0).ReturnsForAnyArgs(fighterPatrolPoints);

            return provider;
        }
	}
}
