namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.TaskProducers.SlotNumber
{
    public interface IPvPSlotNumCalculatorFactory
    {
        IPvPSlotNumCalculator CreateAntiAirSlotNumCalculator(int maxNumOfSlots);
        IPvPSlotNumCalculator CreateAntiNavalSlotNumCalculator(int maxNumOfSlots);
        IPvPSlotNumCalculator CreateStaticSlotNumCalculator(int numOfSlots);
    }
}