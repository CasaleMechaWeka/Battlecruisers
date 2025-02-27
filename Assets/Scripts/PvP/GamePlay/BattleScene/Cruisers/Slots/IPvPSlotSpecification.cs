using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Buildables.Buildings;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots
{
    public interface IPvPSlotSpecification
    {
        BuildingFunction BuildingFunction { get; }
        bool PreferFromFront { get; }
        SlotType SlotType { get; }
    }
}

