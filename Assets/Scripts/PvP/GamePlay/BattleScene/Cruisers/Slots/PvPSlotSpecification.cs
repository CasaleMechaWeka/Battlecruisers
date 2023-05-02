using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots
{
    public class PvPSlotSpecification : IPvPSlotSpecification
    {
        public PvPSlotType SlotType { get; }
        public PvPBuildingFunction BuildingFunction { get; }
        public bool PreferFromFront { get; }

        public PvPSlotSpecification(
            PvPSlotType slotType,
            PvPBuildingFunction buildingFunction = PvPBuildingFunction.Generic,
            bool preferCruiserFront = true)
        {
            SlotType = slotType;
            BuildingFunction = buildingFunction;
            PreferFromFront = preferCruiserFront;
        }

        public override bool Equals(object obj)
        {
            return
                obj is PvPSlotSpecification specification &&
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