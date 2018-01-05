namespace BattleCruisers.AI.TaskProducers.SlotNumber
{
	public class AntiNavalSlotNumCalculator : SlotNumCalculator
	{
		public AntiNavalSlotNumCalculator(int roofSlotNum)
			: base(
                roofSlotNum,
    			slotsForNoThreat: 0,
    			slotsForLowThreat: 2,
    			slotsForHighThreat: 4)
		{
		}
	}
}
