using System.Linq;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Tactical;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;
using NSubstitute;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Balancing
{
    public class KamikazeBalancingTest : BuildableVsBuildableTest
    {
        private KamikazeSignal _kamikazeSignal;

        // FELIX  Merge with DeathstarVsShield?  Identical code :/
        // Create aircraft provider for aircraft
        protected override BuildableInitialisationArgs CreateLeftGroupArgs(Helper helper, Vector2 spawnPosition)
        {
            Vector2 shieldSpawnPosition = new Vector2(spawnPosition.x + LeftOffsetInM + RightOffsetInM, spawnPosition.y);
            IAircraftProvider aircraftProvider
                = new AircraftProvider(parentCruiserPosition: spawnPosition, enemyCruiserPosition: shieldSpawnPosition);

            return
                new BuildableInitialisationArgs(
                    helper,
                    Faction.Blues,
                    parentCruiserDirection: Direction.Right,
                    aircraftProvider: aircraftProvider);
        }

        protected override void OnInitialised()
        {
            _kamikazeSignal = GetComponentInChildren<KamikazeSignal>();
            Assert.IsNotNull(_kamikazeSignal);

            IBuildable rightMostEnemyBuildable = _rightGroup.Buildables.Last();
            ICruiser enemyCruiser = _helper.CreateCruiser(rightMostEnemyBuildable.GameObject);
            enemyCruiser.Faction.Returns(rightMostEnemyBuildable.Faction);
            _helper.InitialiseBuilding(_kamikazeSignal, enemyCruiser: enemyCruiser);

            // Give time for aircraft to start patrolling
            Invoke("Kamikaze", time: 1);
        }

        public void Kamikaze()
        {
            _kamikazeSignal.StartConstruction();
        }
    }
}
