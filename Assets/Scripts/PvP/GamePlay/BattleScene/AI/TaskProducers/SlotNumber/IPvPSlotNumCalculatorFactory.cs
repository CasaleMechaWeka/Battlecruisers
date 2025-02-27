using BattleCruisers.AI.TaskProducers.SlotNumber;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.TaskProducers.SlotNumber
{
    public interface IPvPSlotNumCalculatorFactory
    {
        ISlotNumCalculator CreateAntiAirSlotNumCalculator(int maxNumOfSlots);
        ISlotNumCalculator CreateAntiNavalSlotNumCalculator(int maxNumOfSlots);
        ISlotNumCalculator CreateStaticSlotNumCalculator(int numOfSlots);
    }
}