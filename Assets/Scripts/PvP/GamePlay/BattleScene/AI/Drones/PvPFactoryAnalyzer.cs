using BattleCruisers.AI.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Drones.BuildingMonitors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Utils;
using System.Linq;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Drones
{
    public class PvPFactoryAnalyzer : IFactoryAnalyzer
    {
        private readonly IPvPFactoriesMonitor _factoriesMonitor;
        private readonly IFilter<IPvPFactoryMonitor> _wastingDronesFilter;

        public PvPFactoryAnalyzer(IPvPFactoriesMonitor factoriesMonitor, IFilter<IPvPFactoryMonitor> wastingDronesFilter)
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