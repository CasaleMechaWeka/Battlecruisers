using System.Collections.Generic;
using BattleCruisers.Buildables.Units.Aircraft;
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
            AircraftProvider provider = CreateAircraftProvider();
            helper.InitialiseUnit(_fighter, aircraftProvider: provider);
            _fighter.StartConstruction();
        }

        private AircraftProvider CreateAircraftProvider()
        {
            AircraftProvider provider = Substitute.For<AircraftProvider>();

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

            provider.FighterPatrolPoints(0).ReturnsForAnyArgs(fighterPatrolPoints);

            return provider;
        }
    }
}
