namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.TaskProducers.SlotNumber
{
    public class PvPSlotNumCalculatorFactory : IPvPSlotNumCalculatorFactory
    {
        public IPvPSlotNumCalculator CreateAntiAirSlotNumCalculator(int maxNumOfSlots)
        {
            return new PvPAntiAirSlotNumCalculator(maxNumOfSlots);
        }

        public IPvPSlotNumCalculator CreateAntiNavalSlotNumCalculator(int maxNumOfSlots)
        {
            return new PvPAntiNavalSlotNumCalculator(maxNumOfSlots);
        }

        public IPvPSlotNumCalculator CreateStaticSlotNumCalculator(int numOfSlots)
        {
            return new PvPStaticSlotNumCalculator(numOfSlots);
        }
    }
}
