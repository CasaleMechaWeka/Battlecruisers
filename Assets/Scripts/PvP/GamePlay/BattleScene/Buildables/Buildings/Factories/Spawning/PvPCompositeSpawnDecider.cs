using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories.Spawning
{
    public class PvPCompositeSpawnDecider : IPvPUnitSpawnDecider
    {
        private readonly IPvPUnitSpawnDecider[] _spawnDeciders;

        public PvPCompositeSpawnDecider(params IPvPUnitSpawnDecider[] spawnDeciders)
        {
            Assert.IsNotNull(spawnDeciders);
            Assert.IsTrue(spawnDeciders.Length > 0);

            _spawnDeciders = spawnDeciders;
        }

        public bool CanSpawnUnit(IPvPUnit unitToSpawn)
        {
            foreach (IPvPUnitSpawnDecider decider in _spawnDeciders)
            {
                if (!decider.CanSpawnUnit(unitToSpawn))
                {
                    return false;
                }
            }

            return true;
        }
    }
}