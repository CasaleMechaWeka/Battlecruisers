using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Buildables;

namespace BattleCruisers.Utils.Sorting
{
    /// <summary>
    /// Ordery by:
    /// 1. Drone number (descending)
    /// 2. Then by name
    /// </summary>
    public class DroneAndNameSorter<TBuildable> : IBuildableSorter<TBuildable> 
        where TBuildable : class, IBuildable
    {
        public IList<IBuildableWrapper<TBuildable>> Sort(IList<IBuildableWrapper<TBuildable>> buildables)
        {
            return
                buildables
                    .OrderBy(wrapper => wrapper.Buildable.NumOfDronesRequired)
                    .ThenBy(wrapper => wrapper.Buildable.Name)
                    .ToList();
        }
    }
}
