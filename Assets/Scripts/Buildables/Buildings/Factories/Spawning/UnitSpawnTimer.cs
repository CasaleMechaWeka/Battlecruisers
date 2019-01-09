using BattleCruisers.Utils.PlatformAbstractions;

namespace BattleCruisers.Buildables.Buildings.Factories.Spawning
{
    // FELIX Test :)
    public class UnitSpawnTimer : IUnitSpawnTimer
    {
        private readonly ITime _time;
        private float _timeAtLastUnitStart;

        public float TimeSinceLastUnitStartedInS { get { return _time.TimeSinceGameStartInS - _timeAtLastUnitStart; } }

        public UnitSpawnTimer(IFactory factory, ITime time)
        {
            _time = time;
            _timeAtLastUnitStart = float.MinValue;

            factory.StartedBuildingUnit += Factory_StartedBuildingUnit;
        }

        private void Factory_StartedBuildingUnit(object sender, StartedUnitConstructionEventArgs e)
        {
            _timeAtLastUnitStart = _time.TimeSinceGameStartInS;
        }
    }
}