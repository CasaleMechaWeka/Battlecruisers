namespace BattleCruisers.AI.TaskProducers.SlotNumber
{
    public class SlotNumCalculatorFactory : ISlotNumCalculatorFactory
	{
		public ISlotNumCalculator CreateAntiAirSlotNumCalculator(int maxNumOfSlots)
        {
            return new AntiAirSlotNumCalculator(maxNumOfSlots);
        }

        public ISlotNumCalculator CreateAntiNavalSlotNumCalculator(int maxNumOfSlots)
        {
            return new AntiNavalSlotNumCalculator(maxNumOfSlots);
        }
    }
}
