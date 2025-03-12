using BattleCruisers.Data;
using BattleCruisers.Data.Static;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils.Fetchers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace BattleCruisers.UI.ScreensScene.BattleHubScreen
{
    public class VariantOrganizer
    {
        private readonly IDataProvider _dataProvider;
        private readonly IPrefabFactory _prefabFactory;

        public VariantOrganizer(IDataProvider dataProvider, IPrefabFactory prefabFactory)
        {
            _dataProvider = dataProvider;
            _prefabFactory = prefabFactory;
        }

        public List<int> GetOrganizedVariants()
        {
            var variants = new List<int>();
            
            // First add Building variants
            // Factory buildings
            AddVariantsForBuildingCategory(variants, BuildingCategory.Factory);
            
            // Tactical buildings
            AddVariantsForBuildingCategory(variants, BuildingCategory.Tactical);
            
            // Defence buildings
            AddVariantsForBuildingCategory(variants, BuildingCategory.Defence);
            
            // Offence buildings
            AddVariantsForBuildingCategory(variants, BuildingCategory.Offence);
            
            // Ultra buildings
            AddVariantsForBuildingCategory(variants, BuildingCategory.Ultra);
            
            // Then add Unit variants
            // Aircraft units
            AddVariantsForUnitCategory(variants, UnitCategory.Aircraft);
            
            // Naval units
            AddVariantsForUnitCategory(variants, UnitCategory.Naval);

            return variants;
        }

        private void AddVariantsForBuildingCategory(List<int> variants, BuildingCategory category)
        {
            // Get buildings in this category in their unlock order
            var buildingsInCategory = _dataProvider.GameModel.UnlockedBuildings
                .Where(b => b.BuildingCategory == category)
                .OrderBy(b => _dataProvider.StaticData.BuildingUnlockLevel(b));

            // Add variants for each building
            foreach (var building in buildingsInCategory)
            {
                var buildingVariants = GetVariantsForBuilding(building);
                variants.AddRange(buildingVariants);
            }
        }

        private void AddVariantsForUnitCategory(List<int> variants, UnitCategory category)
        {
            // Get units in this category in their unlock order
            var unitsInCategory = _dataProvider.GameModel.UnlockedUnits
                .Where(u => u.UnitCategory == category)
                .OrderBy(u => _dataProvider.StaticData.UnitUnlockLevel(u));

            // Add variants for each unit
            foreach (var unit in unitsInCategory)
            {
                var unitVariants = GetVariantsForUnit(unit);
                variants.AddRange(unitVariants);
            }
        }

        private List<int> GetVariantsForBuilding(BuildingKey building)
        {
            return _dataProvider.StaticData.Variants
                .Where(v => {
                    var variant = _prefabFactory.GetVariant(
                        StaticPrefabKeys.Variants.GetVariantKey(v.Index));
                    return variant.parent.ToString() == building.PrefabName;
                })
                .Select(v => v.Index)
                .ToList();
        }

        private List<int> GetVariantsForUnit(UnitKey unit)
        {
            return _dataProvider.StaticData.Variants
                .Where(v => {
                    var variant = _prefabFactory.GetVariant(
                        StaticPrefabKeys.Variants.GetVariantKey(v.Index));
                    return variant.parent.ToString() == unit.PrefabName;
                })
                .Select(v => v.Index)
                .ToList();
        }
    }
}