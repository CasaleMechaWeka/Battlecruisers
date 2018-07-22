using BattleCruisers.AI.Drones.BuildingMonitors;
using BattleCruisers.Utils;
using System.Linq;

namespace BattleCruisers.AI.Drones
{
    public class FactoryAnalyzer : IFactoryAnalyzer
    {
        private readonly IFactoriesMonitor _factoriesMonitor;
        private readonly IFilter<IFactoryMonitor> _wastingDronesFilter;

        public FactoryAnalyzer(IFactoriesMonitor factoriesMonitor, IFilter<IFactoryMonitor> wastingDronesFilter)
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