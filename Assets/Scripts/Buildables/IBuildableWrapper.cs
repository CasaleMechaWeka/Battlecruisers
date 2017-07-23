using UnityEngine;

namespace BattleCruisers.Buildables
{
    public interface IBuildableWrapper<TBuildable> where TBuildable : Buildable
	{
        TBuildable Buildable { get; }
        Object UnityObject { get; }
	}
}

