using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories.Spawning
{
    public class PvPUnitSpawnTimer : IPvPUnitSpawnTimer
    {
        private readonly IPvPTime _time;
        private float _timeWhenFactoryWasClearInS, _timeWhenUnitWasChosenInS;

        public float TimeSinceFactoryWasClearInS => _time.TimeSinceGameStartInS - _timeWhenFactoryWasClearInS;
        public float TimeSinceUnitWasChosenInS => _time.TimeSinceGameStartInS - _timeWhenUnitWasChosenInS;

        public PvPUnitSpawnTimer(IPvPFactory factory, IPvPTime time)
        {
            _time = time;
            _timeWhenFactoryWasClearInS = float.MinValue;
            _timeWhenUnitWasChosenInS = float.MinValue;

            factory.UnitStarted += Factory_StartedBuildingUnit;
            factory.UnitCompleted += Factory_CompletedBuildingUnit;
            factory.NewUnitChosen += Factory_NewUnitChosen;
        }

        private void Factory_StartedBuildingUnit(object sender, PvPUnitStartedEventArgs e)
        {
            e.StartedUnit.Destroyed += Buildable_Destroyed;
        }

        private void Buildable_Destroyed(object sender, PvPDestroyedEventArgs e)
        {
            FactoryCleared(e.DestroyedTarget.Parse<IPvPUnit>());
        }

        private void Factory_CompletedBuildingUnit(object sender, PvPUnitCompletedEventArgs e)
        {
            FactoryCleared(e.CompletedUnit);
        }

        private void FactoryCleared(IPvPUnit unitCleared)
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