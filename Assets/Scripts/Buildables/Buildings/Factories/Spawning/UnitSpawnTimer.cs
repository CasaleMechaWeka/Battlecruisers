using BattleCruisers.Buildables.Units;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;

namespace BattleCruisers.Buildables.Buildings.Factories.Spawning
{
    // FELIX Test :)
    public class UnitSpawnTimer : IUnitSpawnTimer
    {
        private readonly ITime _time;
        private float _timeWhenFactoryWasClearInS;

        public float TimeSinceFactoryWasClearInS { get { return _time.TimeSinceGameStartInS - _timeWhenFactoryWasClearInS; } }

        public UnitSpawnTimer(IFactory factory, ITime time)
        {
            _time = time;
            _timeWhenFactoryWasClearInS = float.MinValue;

            factory.StartedBuildingUnit += Factory_StartedBuildingUnit;
            factory.CompletedBuildingUnit += Factory_CompletedBuildingUnit;
        }

        private void Factory_StartedBuildingUnit(object sender, StartedUnitConstructionEventArgs e)
        {
            e.Buildable.Destroyed += Buildable_Destroyed;
        }

        private void Buildable_Destroyed(object sender, DestroyedEventArgs e)
        {
            FactoryCleared(e.DestroyedTarget.Parse<IUnit>());
        }

        private void Factory_CompletedBuildingUnit(object sender, CompletedUnitConstructionEventArgs e)
        {
            FactoryCleared(e.Buildable);
        }

        private void FactoryCleared(IUnit unitCleared)
        {
            _timeWhenFactoryWasClearInS = _time.TimeSinceGameStartInS;
            unitCleared.Destroyed -= Buildable_Destroyed;
        }
    }
}