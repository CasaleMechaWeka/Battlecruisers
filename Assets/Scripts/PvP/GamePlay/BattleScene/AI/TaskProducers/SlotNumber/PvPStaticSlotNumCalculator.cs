namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.TaskProducers.SlotNumber
{
    public class PvPStaticSlotNumCalculator : PvPSlotNumCalculator
    {
        public PvPStaticSlotNumCalculator(int numOfSlots)
            : base(
                numOfSlots,
                slotsForNoThreat: 0,
                slotsForLowThreat: numOfSlots,
                slotsForHighThreat: numOfSlots)
        {
        }
    }
}
