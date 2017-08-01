namespace BattleCruisers.AI.TaskProducers.SlotNumber
{
	public class SlotNumCalculatorFactory : ISlotNumCalculatorFactory
	{
		public ISlotNumCalculator CreateAntiAirSlotNumCalculator(int maxNumOfSlots)
        {
            return new AntiAirSlotNumCalculator(maxNumOfSlots);
        }
	}
}
