using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Threading;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Tactical;

namespace BattleCruisers.AI.ThreatMonitors
{
    public class ThreatMonitorFactory
    {
        private readonly ICruiserController _playerCruiser;
        private readonly ITime _time;
        private readonly IDeferrer _deferrer;

        private const int AIR_HIGH_THREAT_DRONE_NUM = 6;
        private const int NAVAL_HIGH_THREAT_DRONE_NUM = 6;
        private const float ROCKET_LAUNCHER_HIGH_THREAT_BUILDING_NUM = 0.5f;
        private const float STEALTH_GENERATOR_HIGH_THREAT_BUILDING_NUM = 0.5f;

        public ThreatMonitorFactory(ICruiserController playerCruiser, ITime time, IDeferrer deferrer)
        {
            Helper.AssertIsNotNull(playerCruiser, time, deferrer);

            _playerCruiser = playerCruiser;
            _time = time;
            _deferrer = deferrer;
        }

        public BaseThreatMonitor CreateAirThreatMonitor()
        {
            ThreatEvaluator threatEvaluator = new ThreatEvaluator(AIR_HIGH_THREAT_DRONE_NUM);
            return new FactoryThreatMonitor(_playerCruiser, threatEvaluator, UnitCategory.Aircraft);
        }

        public BaseThreatMonitor CreateNavalThreatMonitor()
        {
            ThreatEvaluator threatEvaluator = new ThreatEvaluator(NAVAL_HIGH_THREAT_DRONE_NUM);
            return new FactoryThreatMonitor(_playerCruiser, threatEvaluator, UnitCategory.Naval);
        }

        public BaseThreatMonitor CreateRocketThreatMonitor()
        {
            ThreatEvaluator threatEvaluator = new ThreatEvaluator(ROCKET_LAUNCHER_HIGH_THREAT_BUILDING_NUM);
            return new BuildingThreatMonitor<RocketLauncherController>(_playerCruiser, threatEvaluator);
        }

        public BaseThreatMonitor CreateStealthThreatMonitor()
        {
            ThreatEvaluator threatEvaluator = new ThreatEvaluator(STEALTH_GENERATOR_HIGH_THREAT_BUILDING_NUM);
            return new BuildingThreatMonitor<StealthGenerator>(_playerCruiser, threatEvaluator);
        }

        public BaseThreatMonitor CreateDelayedThreatMonitor(BaseThreatMonitor coreMonitor)
        {
            return new DelayedThreatMonitor(coreMonitor, _time, _deferrer);
        }
    }
}
