namespace BattleCruisers.Buildables
{
    /// <summary>
    /// BuildableWrappers contain:
    /// 1. Buildable
    /// 2. Healthbar
    /// 
    /// This is so that as the buildable's rotation changes (eg, fighter), the healthbar
    /// rotation remains the same.
    /// </summary>
    public interface IBuildableWrapper<TBuildable> where TBuildable : class, IBuildable
	{
        TBuildable Buildable { get; }
        BuildableWrapper<TBuildable> UnityObject { get; }

        void Initialise();
	}
}
