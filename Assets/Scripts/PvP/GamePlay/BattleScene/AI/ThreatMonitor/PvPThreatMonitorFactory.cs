using BattleCruisers.AI.ThreatMonitors;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Tactical;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.ThreatMonitors
{
    public class PvPThreatMonitorFactory : IThreatMonitorFactory
    {
        private readonly PvPCruiser _playerCruiser;
        private readonly ITime _time;
        private readonly IDeferrer _deferrer;

        private const int AIR_HIGH_THREAT_DRONE_NUM = 6;
        private const int NAVAL_HIGH_THREAT_DRONE_NUM = 6;
        private const float ROCKET_LAUNCHER_HIGH_THREAT_BUILDING_NUM = 0.5f;
        private const float STEALTH_GENERATOR_HIGH_THREAT_BUILDING_NUM = 0.5f;

        public PvPThreatMonitorFactory(PvPCruiser playerCruiser, ITime time, IDeferrer deferrer)
        {
            PvPHelper.AssertIsNotNull(playerCruiser, time, deferrer);

            _playerCruiser = playerCruiser;
            _time = time;
            _deferrer = deferrer;
        }

        public IThreatMonitor CreateAirThreatMonitor()
        {
            IThreatEvaluator threatEvaluator = new ThreatEvaluator(AIR_HIGH_THREAT_DRONE_NUM);
            return new PvPFactoryThreatMonitor(_playerCruiser, threatEvaluator, UnitCategory.Aircraft);
        }

        public IThreatMonitor CreateNavalThreatMonitor()
        {
            IThreatEvaluator threatEvaluator = new ThreatEvaluator(NAVAL_HIGH_THREAT_DRONE_NUM);
            return new PvPFactoryThreatMonitor(_playerCruiser, threatEvaluator, UnitCategory.Naval);
        }

        public IThreatMonitor CreateRocketThreatMonitor()
        {
            IThreatEvaluator threatEvaluator = new ThreatEvaluator(ROCKET_LAUNCHER_HIGH_THREAT_BUILDING_NUM);
            return new PvPBuildingThreatMonitor<PvPRocketLauncherController>(_playerCruiser, threatEvaluator);
        }

        public IThreatMonitor CreateStealthThreatMonitor()
        {
            IThreatEvaluator threatEvaluator = new ThreatEvaluator(STEALTH_GENERATOR_HIGH_THREAT_BUILDING_NUM);
            return new PvPBuildingThreatMonitor<IPvPStealthGenerator>(_playerCruiser, threatEvaluator);
        }

        public IThreatMonitor CreateDelayedThreatMonitor(IThreatMonitor coreMonitor)
        {
            return new DelayedThreatMonitor(coreMonitor, _time, _deferrer);
        }
    }
}
