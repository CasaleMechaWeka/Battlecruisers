using BattleCruisers.Buildables.Buildings;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots
{
    public interface IPvPSlotSpecification
    {
        BuildingFunction BuildingFunction { get; }
        bool PreferFromFront { get; }
        PvPSlotType SlotType { get; }
    }
}

