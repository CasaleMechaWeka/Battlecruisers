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
        void AddBuilding(BuildingKey buildingToAdd);
        void RemoveBuilding(BuildingKey buildingToRemove);
	}
}