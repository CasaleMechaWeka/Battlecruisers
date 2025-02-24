using System.Collections.Generic;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models
{
    public interface IPvPLoadout
    {
        PvPHullKey Hull { get; set; }

        IList<PvPBuildingKey> GetBuildings(BuildingCategory buildingCategory);
        IList<PvPUnitKey> GetUnits(UnitCategory unitCategory);

        void AddBuilding(PvPBuildingKey buildingToAdd);
        void RemoveBuilding(PvPBuildingKey buildingToRemove);

        void AddUnit(PvPUnitKey unitToAdd);
        void RemoveUnit(PvPUnitKey unitToRemove);
    }
}
