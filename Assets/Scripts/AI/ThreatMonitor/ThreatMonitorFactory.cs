using BattleCruisers.Buildables.Buildings.Tactical;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;

namespace BattleCruisers.AI.ThreatMonitors
{
    public class ThreatMonitorFactory : IThreatMonitorFactory
    {
		private const int AIR_HIGH_THREAT_DRONE_NUM = 6;
		private const int NAVAL_HIGH_THREAT_DRONE_NUM = 6;
		private const float ROCKET_LAUNCHER_HIGH_THREAT_BUILDING_NUM = 0.5f;
		private const float STEALTH_GENERATOR_HIGH_THREAT_BUILDING_NUM = 0.5f;

		public IThreatMonitor CreateAirThreatMonitor(ICruiserController playerCruiser)
        {
			IThreatEvaluator threatEvaluator = new ThreatEvaluator(AIR_HIGH_THREAT_DRONE_NUM);
			return new FactoryThreatMonitor(playerCruiser, threatEvaluator, UnitCategory.Aircraft);
        }

        public IThreatMonitor CreateNavalThreatMonitor(ICruiserController playerCruiser)
        {
            IThreatEvaluator threatEvaluator = new ThreatEvaluator(NAVAL_HIGH_THREAT_DRONE_NUM);
            return new FactoryThreatMonitor(playerCruiser, threatEvaluator, UnitCategory.Naval);
        }

        public IThreatMonitor CreateRocketThreatMonitor(ICruiserController playerCruiser)
        {
            IThreatEvaluator threatEvaluator = new ThreatEvaluator(ROCKET_LAUNCHER_HIGH_THREAT_BUILDING_NUM);
			return new BuildingThreatMonitor<RocketLauncherController>(playerCruiser, threatEvaluator);
        }

        public IThreatMonitor CreateStealthThreatMonitor(ICruiserController playerCruiser)
        {
            IThreatEvaluator threatEvaluator = new ThreatEvaluator(STEALTH_GENERATOR_HIGH_THREAT_BUILDING_NUM);
            return new BuildingThreatMonitor<IStealthGenerator>(playerCruiser, threatEvaluator);
        }
    }
}
