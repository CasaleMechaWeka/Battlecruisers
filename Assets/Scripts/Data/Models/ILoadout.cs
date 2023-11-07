using System.Collections.Generic;
using System.Threading.Tasks;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Utils.Fetchers;

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
        Task<VariantPrefab> GetSelectedUnitVariant(IPrefabFactory prefabFactory, IUnit unit);
        Task<int> GetSelectedUnitVariantIndex(IPrefabFactory prefabFactory, IUnit unit);
        Task<VariantPrefab> GetSelectedBuildingVariant(IPrefabFactory prefabFactory, IBuilding building);
        Task<int> GetSelectedBuildingVariantIndex(IPrefabFactory prefabFactory, IBuilding building);
    }
}
