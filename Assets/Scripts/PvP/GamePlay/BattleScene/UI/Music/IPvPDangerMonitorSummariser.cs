using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Properties;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Music
{
    public interface IPvPDangerMonitorSummariser
    {
        IPvPBroadcastingProperty<bool> IsInDanger { get; }
    }
}