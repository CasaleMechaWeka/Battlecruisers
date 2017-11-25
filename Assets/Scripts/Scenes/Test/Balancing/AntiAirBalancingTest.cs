using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Targets;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Balancing
{
    public class AntiAirBalancingTest : DefenceBuildingBalancingTest, ITargetConsumer
    {
        private const int BOMBER_CRUISING_ALTITUDE_IN_M = 15;

        public ITarget Target
        {
            set
            {
                if (value == null)
                {
                    OnAllDefenceBuildingsDestroyed();
                }
            }
        }

        protected override IFactory CreateFactory()
        {
            AirFactory factory = GetComponentInChildren<AirFactory>();
            IList<Vector2> bomberPatrolPoints = GetBomberPatrolPoints(factory.transform.position, BOMBER_CRUISING_ALTITUDE_IN_M);
            IAircraftProvider aircraftProvider = _helper.CreateAircraftProvider(bomberPatrolPoints);
            ITargetsFactory targetsFactory = _helper.CreateTargetsFactory(_defenceBuildings);

            // So we know when (if) the bombers manage to destroy all targets
            targetsFactory.BomberTargetProcessor.AddTargetConsumer(this);
            targetsFactory.BomberTargetProcessor.StartProcessingTargets();

            _helper
                .InitialiseBuilding(
                    factory, 
                    Faction.Blues, 
                    parentCruiserDirection: Direction.Right, 
                    aircraftProvider: aircraftProvider,
                    targetsFactory: targetsFactory);

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
