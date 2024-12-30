using BattleCruisers.Utils.Threading;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Threading
{
    public interface IPvPDeferrerProvider
    {
        IDeferrer Deferrer { get; }
        IDeferrer RealTimeDeferrer { get; }
    }
}