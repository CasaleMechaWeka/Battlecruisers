using System.Collections.Generic;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.Data.Models
{
    public interface ILoadout
	{
        HullKey Hull { get; set; }

        IList<BuildingKey> GetBuildings(BuildingCategory buildingCategory);
        IList<UnitKey> GetUnits(UnitCategory unitCategory);

        List<BuildingKey> GetBuildingKeys(BuildingCategory buildingCategory);
        List<UnitKey> GetUnitKeys(UnitCategory unitCategory);

        void AddBuilding(BuildingKey buildingToAdd);
        void RemoveBuilding(BuildingKey buildingToRemove);

        void AddUnit(UnitKey unitToAdd);
        void RemoveUnit(UnitKey unitToRemove);
	}
}
