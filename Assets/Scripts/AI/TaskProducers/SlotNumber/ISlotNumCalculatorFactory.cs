namespace BattleCruisers.AI.TaskProducers.SlotNumber
{
    public interface ISlotNumCalculatorFactory
    {
		ISlotNumCalculator CreateAntiAirSlotNumCalculator(int maxNumOfSlots);
		ISlotNumCalculator CreateAntiNavalSlotNumCalculator(int maxNumOfSlots);
		ISlotNumCalculator CreateStaticSlotNumCalculator(int numOfSlots);
    }
}
