using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Cruisers;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.Drones.BuildingMonitors
{
    public class FactoryMonitor : IFactoryMonitor
    {
        private readonly int _desiredNumOfUnits;
        private int _numOfCompletedUnits;

        public bool HasFactoryBuiltDesiredNumOfUnits { get { return _numOfCompletedUnits >= _desiredNumOfUnits; } }
        public IFactory Factory { get; private set; }

        public FactoryMonitor(IFactory factory, int desiredNumOfUnits)
        {
            Assert.IsNotNull(factory);

            Factory = factory;
            _desiredNumOfUnits = desiredNumOfUnits;

            Factory.CompletedBuildingUnit += Factory_CompletedBuildingUnit;
        }

        private void Factory_CompletedBuildingUnit(object sender, CompletedConstructionEventArgs e)
        {
            _numOfCompletedUnits++;
        }
    }
}
