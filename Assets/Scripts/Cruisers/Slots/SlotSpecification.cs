using BattleCruisers.Buildables.Buildings;

namespace BattleCruisers.Cruisers.Slots
{
    public class  SlotSpecification
    {
        public SlotType SlotType { get; private set; }
        public BuildingFunction BuildingFunction { get; private set; }
        public bool PreferFromFront { get; private set; }

        public SlotSpecification(SlotType slotType, BuildingFunction buildingFunction, bool preferCruiserFront)
        {
            SlotType = slotType;
            BuildingFunction = buildingFunction;
            PreferFromFront = preferCruiserFront;
        }
    }
}