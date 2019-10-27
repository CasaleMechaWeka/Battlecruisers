using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Aircraft
{
    public class BombingSpriteChooserTestGod : TestGodBase
    {
        private AirFactory _factory;
        private BomberController _bomber;

        public List<Vector2> patrolPoints;

        protected override List<GameObject> GetGameObjects()
        {
            _factory = FindObjectOfType<AirFactory>();
            _bomber = FindObjectOfType<BomberController>();

            return new List<GameObject>()
            {
                _factory.GameObject,
                _bomber.GameObject
            };
        }

        protected override void Setup(Helper helper)
        {
            // Factory
            helper.InitialiseBuilding(_factory, Faction.Blues);

            // Bomber
            IList<TargetType> targetTypes = new List<TargetType>() { _factory.TargetType };
            ITargetFilter targetFilter = new FactionAndTargetTypeFilter(_factory.Faction, targetTypes);
            ITargetFactories targetFactories = helper.CreateTargetFactories(_factory.GameObject, targetFilter: targetFilter);

            IAircraftProvider aircraftProvider = helper.CreateAircraftProvider(bomberPatrolPoints: patrolPoints);
            helper.InitialiseUnit(_bomber, Faction.Reds, aircraftProvider: aircraftProvider, targetFactories: targetFactories, parentCruiserDirection: Direction.Right);
            _bomber.StartConstruction();
        }
    }
}
