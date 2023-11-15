namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.ThreatMonitors
{
    public interface IPvPThreatEvaluator
    {
        PvPThreatLevel FindThreatLevel(float value);
    }
}