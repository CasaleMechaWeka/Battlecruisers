using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Drones.BuildingMonitors
{
    public class PvPFactoryMonitor : IPvPFactoryMonitor
    {
        private readonly int _desiredNumOfUnits;
        private int _numOfCompletedUnits;

        public bool HasFactoryBuiltDesiredNumOfUnits => _numOfCompletedUnits >= _desiredNumOfUnits;
        public IPvPFactory Factory { get; }

        public PvPFactoryMonitor(IPvPFactory factory, int desiredNumOfUnits)
        {
            Assert.IsNotNull(factory);

            Factory = factory;
            _desiredNumOfUnits = desiredNumOfUnits;

            Factory.UnitCompleted += Factory_CompletedBuildingUnit;
        }

        private void Factory_CompletedBuildingUnit(object sender, PvPUnitCompletedEventArgs e)
        {
            _numOfCompletedUnits++;
        }
    }
}
