using BattleCruisers.AI.ThreatMonitors;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.ThreatMonitors
{
    public interface IPvPThreatEvaluator
    {
        ThreatLevel FindThreatLevel(float value);
    }
}