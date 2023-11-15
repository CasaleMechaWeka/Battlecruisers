namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.DataStrctures
{
    public interface IPvPRange<T>
    {
        T Min { get; }
        T Max { get; }
    }
}
