using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Utils;

namespace BattleCruisers.Cruisers.Slots
{
    public class SlotSpecification : ISlotSpecification
    {
        public SlotType SlotType { get; }
        public BuildingFunction BuildingFunction { get; }
        public bool PreferFromFront { get; }

        public SlotSpecification(
            SlotType slotType,
            BuildingFunction buildingFunction = BuildingFunction.Generic,
            bool preferCruiserFront = true)
        {
            SlotType = slotType;
            BuildingFunction = buildingFunction;
            PreferFromFront = preferCruiserFront;
        }

        public override bool Equals(object obj)
        {
            return 
                obj is SlotSpecification specification &&
                SlotType == specification.SlotType &&
                BuildingFunction == specification.BuildingFunction &&
                PreferFromFront == specification.PreferFromFront;
        }

        public override int GetHashCode()
        {
            return this.GetHashCode(SlotType, BuildingFunction, PreferFromFront);
        }
    }
}