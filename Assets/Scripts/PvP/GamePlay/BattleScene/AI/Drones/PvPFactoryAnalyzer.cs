using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Drones.BuildingMonitors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System.Linq;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Drones
{
    public class PvPFactoryAnalyzer : IPvPFactoryAnalyzer
    {
        private readonly IPvPFactoriesMonitor _factoriesMonitor;
        private readonly IPvPFilter<IPvPFactoryMonitor> _wastingDronesFilter;

        public PvPFactoryAnalyzer(IPvPFactoriesMonitor factoriesMonitor, IPvPFilter<IPvPFactoryMonitor> wastingDronesFilter)
        {
            PvPHelper.AssertIsNotNull(factoriesMonitor, wastingDronesFilter);

            _factoriesMonitor = factoriesMonitor;
            _wastingDronesFilter = wastingDronesFilter;
        }

        public bool AreAnyFactoriesWronglyUsingDrones
        {
            get
            {
                return
                    _factoriesMonitor
                        .CompletedFactories
                        .Any(_wastingDronesFilter.IsMatch);
            }
        }
    }
}