using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Balancing.Defensives
{
    public class AntiAirBalancingTest : DefenceBuildingBalancingTest
    {
        private const int BOMBER_CRUISING_ALTITUDE_IN_M = 15;

        protected override IFactory CreateFactory(IList<ITarget> defenceBuildings)
        {
            AirFactory factory = GetComponentInChildren<AirFactory>();
            IList<Vector2> bomberPatrolPoints = GetBomberPatrolPoints(factory.transform.position, BOMBER_CRUISING_ALTITUDE_IN_M);
            IAircraftProvider aircraftProvider = _helper.CreateAircraftProvider(bomberPatrolPoints);
            ITargetFactories targetFactories = _helper.CreateTargetFactories(new ObservableCollection<ITarget>(defenceBuildings));

            _helper
                .InitialiseBuilding(
                    factory, 
                    Faction.Blues, 
                    parentCruiserDirection: Direction.Right, 
                    aircraftProvider: aircraftProvider,
                    targetFactories: targetFactories);

            return factory;
        }

        private IList<Vector2> GetBomberPatrolPoints(Vector2 factoryPosition, float bomberCruisingAltitudeInM)
        {
            return new List<Vector2>()
            {
                new Vector2(factoryPosition.x, bomberCruisingAltitudeInM),
                new Vector2(DEFENCE_BUILDINGS_OFFSET_IN_M, bomberCruisingAltitudeInM)
            };
        }    
    }
}
