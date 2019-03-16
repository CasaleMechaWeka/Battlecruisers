using BattleCruisers.Buildables.Buildings;

namespace BattleCruisers.Cruisers.Slots
{
    public class  SlotSpecification
    {
        public SlotType SlotType { get; }
        public BuildingFunction BuildingFunction { get; }
        public bool PreferFromFront { get; }

        public SlotSpecification(SlotType slotType, BuildingFunction buildingFunction, bool preferCruiserFront)
        {
            SlotType = slotType;
            BuildingFunction = buildingFunction;
            PreferFromFront = preferCruiserFront;
        }
    }
}