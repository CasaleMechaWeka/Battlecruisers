using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Balancing
{
    public class KamikazeBalancingTest : BuildableVsBuildableTest
    {
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
    }
}
