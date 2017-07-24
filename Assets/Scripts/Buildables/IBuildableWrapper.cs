namespace BattleCruisers.Buildables
{
    public interface IBuildableWrapper<TBuildable> where TBuildable : class, IBuildable
	{
        TBuildable Buildable { get; }
        BuildableWrapper<TBuildable> UnityObject { get; }

        void Initialise();
	}
}
