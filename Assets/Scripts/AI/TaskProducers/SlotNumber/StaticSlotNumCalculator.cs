namespace BattleCruisers.AI.TaskProducers.SlotNumber
{
    public class StaticSlotNumCalculator : SlotNumCalculator
    {
        public StaticSlotNumCalculator(int numOfSlots)
            : base(numOfSlots, 
            slotsForNoThreat: 0, 
            slotsForLowThreat: numOfSlots, 
            slotsForHighThreat: numOfSlots)
        {
        }
    }
}
