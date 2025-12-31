using BattleCruisers.Buildables.Buildings;

namespace BattleCruisers.Cruisers.Slots
{
    public interface ISlotSpecification
    {
        BuildingFunction BuildingFunction { get; }
        bool PreferFromFront { get; }
        SlotType SlotType { get; }
    }
}