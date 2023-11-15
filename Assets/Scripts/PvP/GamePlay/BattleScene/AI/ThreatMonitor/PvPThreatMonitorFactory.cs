using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Tactical;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Threading;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.ThreatMonitors
{
    public class PvPThreatMonitorFactory : IPvPThreatMonitorFactory
    {
        private readonly PvPCruiser _playerCruiser;
        private readonly IPvPTime _time;
        private readonly IPvPDeferrer _deferrer;

        private const int AIR_HIGH_THREAT_DRONE_NUM = 6;
        private const int NAVAL_HIGH_THREAT_DRONE_NUM = 6;
        private const float ROCKET_LAUNCHER_HIGH_THREAT_BUILDING_NUM = 0.5f;
        private const float STEALTH_GENERATOR_HIGH_THREAT_BUILDING_NUM = 0.5f;

        public PvPThreatMonitorFactory(PvPCruiser playerCruiser, IPvPTime time, IPvPDeferrer deferrer)
        {
            PvPHelper.AssertIsNotNull(playerCruiser, time, deferrer);

            _playerCruiser = playerCruiser;
            _time = time;
            _deferrer = deferrer;
        }

        public IPvPThreatMonitor CreateAirThreatMonitor()
        {
            IPvPThreatEvaluator threatEvaluator = new PvPThreatEvaluator(AIR_HIGH_THREAT_DRONE_NUM);
            return new PvPFactoryThreatMonitor(_playerCruiser, threatEvaluator, PvPUnitCategory.Aircraft);
        }

        public IPvPThreatMonitor CreateNavalThreatMonitor()
        {
            IPvPThreatEvaluator threatEvaluator = new PvPThreatEvaluator(NAVAL_HIGH_THREAT_DRONE_NUM);
            return new PvPFactoryThreatMonitor(_playerCruiser, threatEvaluator, PvPUnitCategory.Naval);
        }

        public IPvPThreatMonitor CreateRocketThreatMonitor()
        {
            IPvPThreatEvaluator threatEvaluator = new PvPThreatEvaluator(ROCKET_LAUNCHER_HIGH_THREAT_BUILDING_NUM);
            return new PvPBuildingThreatMonitor<PvPRocketLauncherController>(_playerCruiser, threatEvaluator);
        }

        public IPvPThreatMonitor CreateStealthThreatMonitor()
        {
            IPvPThreatEvaluator threatEvaluator = new PvPThreatEvaluator(STEALTH_GENERATOR_HIGH_THREAT_BUILDING_NUM);
            return new PvPBuildingThreatMonitor<IPvPStealthGenerator>(_playerCruiser, threatEvaluator);
        }

        public IPvPThreatMonitor CreateDelayedThreatMonitor(IPvPThreatMonitor coreMonitor)
        {
            return new PvPDelayedThreatMonitor(coreMonitor, _time, _deferrer);
        }
    }
}
