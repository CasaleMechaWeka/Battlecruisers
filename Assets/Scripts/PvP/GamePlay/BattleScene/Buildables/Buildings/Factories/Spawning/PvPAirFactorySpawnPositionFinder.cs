using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories.Spawning
{
    public class PvPAirFactorySpawnPositionFinder : IPvPUnitSpawnPositionFinder
    {
        private readonly IPvPFactory _factory;

        public PvPAirFactorySpawnPositionFinder(IPvPFactory factory)
        {
            Assert.IsNotNull(factory);
            _factory = factory;
        }

        public Vector3 FindSpawnPosition(IPvPUnit unitToSpawn)
        {
            float verticalChange = (_factory.Size.y * 0.7f) + (unitToSpawn.Size.y * 0.5f);
            return _factory.Transform.Position + (_factory.Transform.Up * verticalChange);
        }
    }
}