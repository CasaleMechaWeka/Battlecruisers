namespace BattleCruisers.AI.TaskProducers.SlotNumber
{
    public interface ISlotNumCalculatorFactory
    {
        ISlotNumCalculator CreateAntiAirSlotNumCalculator(int maxNumOfSlots);
    }
}
