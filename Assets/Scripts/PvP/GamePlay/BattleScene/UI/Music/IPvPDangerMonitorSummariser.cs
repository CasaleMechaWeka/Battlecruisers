using BattleCruisers.Utils.Properties;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Music
{
    public interface IPvPDangerMonitorSummariser
    {
        IBroadcastingProperty<bool> IsInDanger { get; }
    }
}