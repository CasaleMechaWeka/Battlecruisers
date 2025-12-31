using System.Collections.Generic;
using BattleCruisers.Buildables;

namespace BattleCruisers.Utils.Sorting
{
    public interface IBuildableSorter<TBuildable> where TBuildable : class, IBuildable
    {
        IList<IBuildableWrapper<TBuildable>> Sort(IList<IBuildableWrapper<TBuildable>> buildables);
    }
}
