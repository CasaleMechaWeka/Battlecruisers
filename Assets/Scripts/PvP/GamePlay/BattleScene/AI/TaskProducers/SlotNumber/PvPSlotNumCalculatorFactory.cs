using BattleCruisers.AI.TaskProducers.SlotNumber;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.TaskProducers.SlotNumber
{
    public class PvPSlotNumCalculatorFactory : IPvPSlotNumCalculatorFactory
    {
        public ISlotNumCalculator CreateAntiAirSlotNumCalculator(int maxNumOfSlots)
        {
            return new PvPAntiAirSlotNumCalculator(maxNumOfSlots);
        }

        public ISlotNumCalculator CreateAntiNavalSlotNumCalculator(int maxNumOfSlots)
        {
            return new PvPAntiNavalSlotNumCalculator(maxNumOfSlots);
        }

        public ISlotNumCalculator CreateStaticSlotNumCalculator(int numOfSlots)
        {
            return new PvPStaticSlotNumCalculator(numOfSlots);
        }
    }
}
