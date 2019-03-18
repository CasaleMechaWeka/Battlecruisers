using BattleCruisers.Buildables.Units;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using System;

namespace BattleCruisers.Buildables.Buildings.Factories.Spawning
{
    public class UnitSpawnTimer : IUnitSpawnTimer
    {
        private readonly ITime _time;
        private float _timeWhenFactoryWasClearInS, _timeWhenUnitWasChosenInS;

        public float TimeSinceFactoryWasClearInS => _time.TimeSinceGameStartInS - _timeWhenFactoryWasClearInS;
        public float TimeSinceUnitWasChosenInS => _time.TimeSinceGameStartInS - _timeWhenUnitWasChosenInS;

        public UnitSpawnTimer(IFactory factory, ITime time)
        {
            _time = time;
            _timeWhenFactoryWasClearInS = float.MinValue;
            _timeWhenUnitWasChosenInS = float.MinValue;

            factory.StartedBuildingUnit += Factory_StartedBuildingUnit;
            factory.CompletedBuildingUnit += Factory_CompletedBuildingUnit;
            factory.NewUnitChosen += Factory_NewUnitChosen;
        }

        private void Factory_StartedBuildingUnit(object sender, UnitStartedEventArgs e)
        {
            e.StartedUnit.Destroyed += Buildable_Destroyed;
        }

        private void Buildable_Destroyed(object sender, DestroyedEventArgs e)
        {
            FactoryCleared(e.DestroyedTarget.Parse<IUnit>());
        }

        private void Factory_CompletedBuildingUnit(object sender, UnitCompletedEventArgs e)
        {
            FactoryCleared(e.CompletedUnit);
        }

        private void FactoryCleared(IUnit unitCleared)
        {
            _timeWhenFactoryWasClearInS = _time.TimeSinceGameStartInS;
            unitCleared.Destroyed -= Buildable_Destroyed;
        }

        private void Factory_NewUnitChosen(object sender, EventArgs e)
        {
            _timeWhenUnitWasChosenInS = _time.TimeSinceGameStartInS;
        }
    }
}