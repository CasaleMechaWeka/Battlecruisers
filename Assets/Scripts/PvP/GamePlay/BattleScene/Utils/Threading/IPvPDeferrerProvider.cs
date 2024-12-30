namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Threading
{
    public interface IPvPDeferrerProvider
    {
        IPvPDeferrer Deferrer { get; }
        IPvPDeferrer RealTimeDeferrer { get; }
    }
}