namespace BattleCruisers.Buildables
{
    public interface IBuildableWrapper<TBuildable> where TBuildable : Buildable
	{
        TBuildable Buildable { get; }
        BuildableWrapper<TBuildable> UnityObject { get; }
	}
}

