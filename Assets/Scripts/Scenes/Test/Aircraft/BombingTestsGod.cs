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
    public class BombingTestsGod : TestGodBase
	{
        private AirFactory _factory;

        public BomberController bomberToLeft, bomberToRight;
		public List<Vector2> leftPatrolPoints, rightPatrolPoints;

        protected override IList<GameObject> GetGameObjects()
        {
			_factory = FindObjectOfType<AirFactory>();

            return new List<GameObject>()
            {
                _factory.GameObject,
                bomberToLeft.GameObject,
                bomberToRight.GameObject
            };
        }

        protected override void Setup(Helper helper)
        {
            // Factory
            helper.InitialiseBuilding(_factory, Faction.Blues);

            // Bombers
            IList<TargetType> targetTypes = new List<TargetType>() { _factory.TargetType };
            ITargetFilter targetFilter = new FactionAndTargetTypeFilter(_factory.Faction, targetTypes);
            ITargetFactories targetFactories = helper.CreateTargetFactories(_factory.GameObject, targetFilter: targetFilter);

            IAircraftProvider leftAircraftProvider = helper.CreateAircraftProvider(bomberPatrolPoints: leftPatrolPoints);
            helper.InitialiseUnit(bomberToLeft, Faction.Reds, aircraftProvider: leftAircraftProvider, targetFactories: targetFactories, parentCruiserDirection: Direction.Right);
            bomberToLeft.StartConstruction();

            IAircraftProvider rightAircraftProvider = helper.CreateAircraftProvider(bomberPatrolPoints: rightPatrolPoints);
            helper.InitialiseUnit(bomberToRight, Faction.Reds, aircraftProvider: rightAircraftProvider, targetFactories: targetFactories, parentCruiserDirection: Direction.Left);
            bomberToRight.StartConstruction();
        }
    }
}
