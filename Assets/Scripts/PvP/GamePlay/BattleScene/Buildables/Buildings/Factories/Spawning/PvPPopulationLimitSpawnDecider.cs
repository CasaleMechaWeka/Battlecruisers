using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories.Spawning
{
    public class PvPPopulationLimitSpawnDecider : IPvPUnitSpawnDecider
    {
        private readonly IPvPCruiserUnitMonitor _unitMonitor;
        private readonly int _populationLimit;

        public PvPPopulationLimitSpawnDecider(IPvPCruiserUnitMonitor unitMonitor, int populationLimit)
        {
            Assert.IsNotNull(unitMonitor);
            Assert.IsTrue(populationLimit > 0);

            _unitMonitor = unitMonitor;
            _populationLimit = populationLimit;
        }

        public bool CanSpawnUnit(IPvPUnit unitToSpawn)
        {
            return _unitMonitor.AliveUnits.Count < _populationLimit;
        }
    }
}