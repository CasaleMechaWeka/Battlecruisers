namespace BattleCruisers.AI.TaskProducers.SlotNumber
{
	public class AntiNavalSlotNumCalculator : SlotNumCalculator
	{
		public AntiNavalSlotNumCalculator(int numOfSlots)
			: base(numOfSlots,
			slotsForNoThreat: 0,
			slotsForLowThreat: 2,
			slotsForHighThreat: 4)
		{
		}
	}
}
