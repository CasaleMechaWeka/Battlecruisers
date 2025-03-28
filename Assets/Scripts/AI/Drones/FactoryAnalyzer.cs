using BattleCruisers.AI.Drones.BuildingMonitors;
using BattleCruisers.Utils;
using System.Linq;

namespace BattleCruisers.AI.Drones
{
    public class FactoryAnalyzer : IFactoryAnalyzer
    {
        private readonly FactoriesMonitor _factoriesMonitor;
        private readonly IFilter<FactoryMonitor> _wastingDronesFilter;

        public FactoryAnalyzer(FactoriesMonitor factoriesMonitor, IFilter<FactoryMonitor> wastingDronesFilter)
        {
            Helper.AssertIsNotNull(factoriesMonitor, wastingDronesFilter);

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