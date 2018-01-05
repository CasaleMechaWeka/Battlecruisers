namespace BattleCruisers.AI.TaskProducers.SlotNumber
{
	public class AntiAirSlotNumCalculator : SlotNumCalculator
	{
		public AntiAirSlotNumCalculator(int roofSlotNum)
			: base(
                roofSlotNum,
    			slotsForNoThreat: 0,
    			slotsForLowThreat: 2,
    			slotsForHighThreat: 4)
		{
		}
	}
}
