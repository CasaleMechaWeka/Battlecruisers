namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables
{
    public interface IPvPBuildableWrapper<TBuildable> : IPrefab where TBuildable : class, IPvPBuildable
    {
        TBuildable Buildable { get; }
        PvPBuildableWrapper<TBuildable> UnityObject { get; }
    }
}


