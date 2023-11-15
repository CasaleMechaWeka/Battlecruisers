namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.TaskProducers.SlotNumber
{
    public class PvPAntiAirSlotNumCalculator : PvPSlotNumCalculator
    {
        public PvPAntiAirSlotNumCalculator(int roofSlotNum)
            : base(
                roofSlotNum,
                slotsForNoThreat: 0,
                slotsForLowThreat: 2,
                slotsForHighThreat: 4)
        {
        }
    }
}
