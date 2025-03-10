using BattleCruisers.Data;
using BattleCruisers.Data.Static;
using BattleCruisers.Buildables.Units;
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
            
            // First Aircraft
            AddVariantsForCategory(variants, UnitCategory.Aircraft);
            
            // Then Naval
            AddVariantsForCategory(variants, UnitCategory.Naval);

            return variants;
        }

        private void AddVariantsForCategory(List<int> variants, UnitCategory category)
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