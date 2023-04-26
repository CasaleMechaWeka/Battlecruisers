namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils
{
    public interface IPvPFilter<TElement>
    {
        bool IsMatch(TElement element);
    }
}
