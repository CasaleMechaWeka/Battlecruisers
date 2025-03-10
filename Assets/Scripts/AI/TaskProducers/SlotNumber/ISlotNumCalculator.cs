using BattleCruisers.AI.ThreatMonitors;

namespace BattleCruisers.AI.TaskProducers.SlotNumber
{
    public interface ISlotNumCalculator
    {
        int FindSlotNum(ThreatLevel threatLevel);
    }
}