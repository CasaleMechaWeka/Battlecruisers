using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.ThreatMonitors;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.TaskProducers.SlotNumber
{
    public interface IPvPSlotNumCalculator
    {
        int FindSlotNum(PvPThreatLevel threatLevel);
    }
}