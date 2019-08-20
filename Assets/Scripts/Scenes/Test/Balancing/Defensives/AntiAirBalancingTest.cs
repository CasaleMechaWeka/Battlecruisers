using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Construction;
using NSubstitute;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Balancing.Defensives
{
    public class AntiAirBalancingTest : DefenceBuildingBalancingTest
    {
        private const int BOMBER_CRUISING_ALTITUDE_IN_M = 15;

        // FELIX  Still need separate implementation compared to AntiSeaBalancingTest?
        protected override IFactory CreateFactory(ICruiser enemyCruiser)
        {
            AirFactory factory = GetComponentInChildren<AirFactory>();
            IList<Vector2> bomberPatrolPoints = GetBomberPatrolPoints(factory.transform.position, BOMBER_CRUISING_ALTITUDE_IN_M);
            IAircraftProvider aircraftProvider = _helper.CreateAircraftProvider(bomberPatrolPoints);
            // FELIX  Try use natural target detection instead :)
            //ITargetFactories targetFactories = _helper.CreateTargetFactories(new ObservableCollection<ITarget>(defenceBuildings));

            _helper
                .InitialiseBuilding(
                    factory, 
                    Faction.Blues, 
                    parentCruiserDirection: Direction.Right, 
                    aircraftProvider: aircraftProvider,
                    enemyCruiser: enemyCruiser);

            return factory;
        }

        private void DefenceBuilding_BuildableProgress(object sender, BuildProgressEventArgs e)
        {
            throw new System.NotImplementedException();
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
