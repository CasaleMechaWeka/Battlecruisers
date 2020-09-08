using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Buildables;

namespace BattleCruisers.Utils.Sorting
{
    /// <summary>
    /// Ordery by:
    /// 1. [Cost part 1]  Drone number (descending)
    /// 2. [Cost part 2]  Build time in seconds
    /// 3. Name
    /// </summary>
    /// FELIX  Sort by when is unlocked instead :)
    public class CostAndNameSorter<TBuildable> : IBuildableSorter<TBuildable> 
        where TBuildable : class, IBuildable
    {
        public IList<IBuildableWrapper<TBuildable>> Sort(IList<IBuildableWrapper<TBuildable>> buildables)
        {
            return
                buildables
                    .OrderBy(wrapper => wrapper.Buildable.NumOfDronesRequired)
                    .ThenBy(wrapper => wrapper.Buildable.BuildTimeInS)
                    .ThenBy(wrapper => wrapper.Buildable.Name)
                    .ToList();
        }
    }
}
