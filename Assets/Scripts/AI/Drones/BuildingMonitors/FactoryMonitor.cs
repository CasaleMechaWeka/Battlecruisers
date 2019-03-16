using BattleCruisers.Buildables.Buildings.Factories;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.Drones.BuildingMonitors
{
    public class FactoryMonitor : IFactoryMonitor
    {
        private readonly int _desiredNumOfUnits;
        private int _numOfCompletedUnits;

        public bool HasFactoryBuiltDesiredNumOfUnits { get { return _numOfCompletedUnits >= _desiredNumOfUnits; } }
        public IFactory Factory { get; }

        public FactoryMonitor(IFactory factory, int desiredNumOfUnits)
        {
            Assert.IsNotNull(factory);

            Factory = factory;
            _desiredNumOfUnits = desiredNumOfUnits;

            Factory.CompletedBuildingUnit += Factory_CompletedBuildingUnit;
        }

        private void Factory_CompletedBuildingUnit(object sender, CompletedUnitConstructionEventArgs e)
        {
            _numOfCompletedUnits++;
        }
    }
}
