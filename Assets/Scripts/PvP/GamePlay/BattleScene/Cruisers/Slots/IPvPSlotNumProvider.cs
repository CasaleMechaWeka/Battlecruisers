using BattleCruisers.Cruisers.Slots;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots
{
    public interface IPvPSlotNumProvider
    {
        int GetSlotCount(SlotType type);
    }
}
