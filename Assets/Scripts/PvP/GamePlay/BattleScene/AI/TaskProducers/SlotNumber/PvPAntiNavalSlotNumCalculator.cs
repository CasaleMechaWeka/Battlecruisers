namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.TaskProducers.SlotNumber
{
    public class PvPAntiNavalSlotNumCalculator : PvPSlotNumCalculator
    {
        public PvPAntiNavalSlotNumCalculator(int roofSlotNum)
            : base(
                roofSlotNum,
                slotsForNoThreat: 0,
                slotsForLowThreat: 2,
                slotsForHighThreat: 4)
        {
        }
    }
}
