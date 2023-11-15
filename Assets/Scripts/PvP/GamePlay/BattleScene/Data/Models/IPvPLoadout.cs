using System.Collections.Generic;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models
{
    public interface IPvPLoadout
    {
        PvPHullKey Hull { get; set; }

        IList<PvPBuildingKey> GetBuildings(PvPBuildingCategory buildingCategory);
        IList<PvPUnitKey> GetUnits(PvPUnitCategory unitCategory);

        void AddBuilding(PvPBuildingKey buildingToAdd);
        void RemoveBuilding(PvPBuildingKey buildingToRemove);

        void AddUnit(PvPUnitKey unitToAdd);
        void RemoveUnit(PvPUnitKey unitToRemove);
    }
}
