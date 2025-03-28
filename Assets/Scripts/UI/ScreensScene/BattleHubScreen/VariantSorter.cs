using BattleCruisers.Data;
using BattleCruisers.Data.Static;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.UI.ScreensScene.ShopScreen;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;

namespace BattleCruisers.UI.ScreensScene.BattleHubScreen
{
    // Organizes unit variants into a consistent display order.
    // - Groups variants by their parent unit
    // - Sorts groups by category (Buildings -> Aircraft -> Naval)
    // - Orders variants within groups by their type (QuickBuild, RapidFire, etc.)
    public class VariantSorter
    {
        private readonly Dictionary<int, string> _variantParentCache;
        private readonly object _cacheLock = new object();

        // Defines the display order for variant types within each parent group
        // Lower numbers appear first in the list
        private static readonly Dictionary<string, int> NameKeyOrder = new Dictionary<string, int>
        {
            {"QuickBuild", 0},
            {"RapidFire", 1},
            {"DoubleShot", 2},
            {"TripleShot", 3},
            {"Damaging", 4},
            {"LongRange", 5},
            {"Sniper", 6},
            {"Robust", 7},
            {"Refined", 8}
        };

        public VariantSorter()
        {
            _variantParentCache = new Dictionary<int, string>();
        }

        // Returns a list of variant indices organized by category and type.
        // Buildings are listed first, followed by Aircraft, then Naval units.
        // Within each parent group, variants are sorted by their NameKey order.
        public List<int> GetOrganizedVariants()
        {
            try
            {
                var allVariants = StaticData.Variants;
                if (allVariants == null || allVariants.Count == 0)
                {
                    Debug.LogWarning("[VariantSorter] No variants found");
                    return new List<int>();
                }

                // Filter for unlocked items first
                var unlockedVariants = allVariants.Where(v =>
                {
                    try
                    {
                        var prefab = PrefabFactory.GetVariant(
                            StaticPrefabKeys.Variants.GetVariantKey(v.Index));

                        if (prefab == null || prefab.parent == null)
                            return false;

                        var parentName = prefab.parent.ToString().ToLowerInvariant();

                        // Check if parent is a building
                        if (DataProvider.GameModel.UnlockedBuildings.Any(b =>
                            b.PrefabName.ToLowerInvariant() == parentName))
                            return true;

                        // Check if parent is a unit
                        if (DataProvider.GameModel.UnlockedUnits.Any(u =>
                            u.PrefabName.ToLowerInvariant() == parentName))
                            return true;

                        return false;
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError($"[VariantSorter] Error checking unlock status for variant {v.Index}: {e}");
                        return false;
                    }
                }).ToList();

                if (unlockedVariants.Count == 0)
                {
                    Debug.LogWarning("[VariantSorter] No unlocked variants found");
                    return new List<int>();
                }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
                LogExampleVariant(unlockedVariants[0]);
#endif

                CacheVariantParents(unlockedVariants);

                // Log how many variants we found before categorizing
                Debug.Log($"[VariantSorter] Found {unlockedVariants.Count} unlocked variants");

                var uncachedVariants = unlockedVariants.Where(v => !_variantParentCache.ContainsKey(v.Index)).ToList();
                if (uncachedVariants.Any())
                {
                    Debug.LogWarning($"[VariantSorter] Found {uncachedVariants.Count} variants without parents");
                }

                var (buildingVariants, aircraftVariants, navalVariants) = CategorizeVariants(unlockedVariants);

                var result = buildingVariants
                    .Concat(aircraftVariants)
                    .Concat(navalVariants)
                    .ToList();

#if UNITY_EDITOR || DEVELOPMENT_BUILD
                // Debug logging only - this block is completely removed in production
                var parentCounts = result.GroupBy(v => _variantParentCache[v])
                    .OrderBy(g => g.Key)
                    .ToDictionary(g => g.Key, g => g.Count());

                // Get all possible parent groups from StaticPrefabKeys
                var allParentGroups = new HashSet<string>();

                // Add Buildings
                foreach (var building in StaticPrefabKeys.Buildings.AllKeys)
                {
                    allParentGroups.Add(building.PrefabName.ToLowerInvariant());
                }

                // Add Units
                foreach (var unit in StaticPrefabKeys.Units.AllKeys)
                {
                    allParentGroups.Add(unit.PrefabName.ToLowerInvariant());
                }

                var sb = new StringBuilder();
                sb.AppendLine($"[VariantSorter] Returning {result.Count} organized variants:");
                sb.AppendLine("Parent groups:");

                // First list groups with variants
                foreach (var kvp in parentCounts.OrderBy(kvp => kvp.Key))
                {
                    sb.AppendLine($"  {kvp.Key}: {kvp.Value} variants");
                    allParentGroups.Remove(kvp.Key); // Remove from all groups as we've handled it
                }

                // Then list groups with 0 variants
                foreach (var group in allParentGroups.OrderBy(g => g))
                {
                    sb.AppendLine($"  {group}: 0 variants");
                }

                Debug.Log(sb.ToString());
#endif

                // If we lost variants in the process, log a warning
                if (result.Count < unlockedVariants.Count - uncachedVariants.Count)
                {
                    Debug.LogWarning($"[VariantSorter] Lost {unlockedVariants.Count - uncachedVariants.Count - result.Count} variants during organization");
                }

                return result;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[VariantSorter] Critical error: {e}");
                // Instead of returning empty, return all valid variant indices we can find
                return StaticData.Variants?
                    .Where(v => v != null)
                    .Select(v => v.Index)
                    .ToList() ?? new List<int>();
            }
        }

        private void CacheVariantParents(IReadOnlyList<VariantData> variants)
        {
            lock (_cacheLock)
            {
                if (_variantParentCache.Count > 0) return;

                foreach (var variant in variants)
                {
                    try
                    {
                        var prefab = PrefabFactory.GetVariant(
                            StaticPrefabKeys.Variants.GetVariantKey(variant.Index));

                        if (prefab == null)
                        {
                            Debug.LogWarning($"[VariantSorter] Null prefab for variant {variant.Index}");
                            continue;
                        }

                        if (prefab.parent == null)
                        {
                            Debug.LogWarning($"[VariantSorter] Null parent for variant {variant.Index}");
                            continue;
                        }

                        _variantParentCache[variant.Index] = prefab.parent.ToString().ToLowerInvariant();
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError($"[VariantSorter] Failed to cache variant {variant.Index}: {e}");
                    }
                }

                Debug.Log($"[VariantSorter] Cached {_variantParentCache.Count}/{variants.Count} variant parents");
            }
        }

        // Categorizes variants into building, aircraft, and naval groups.
        // Within each category, variants are grouped by parent unit and sorted by type.
        private (List<int> building, List<int> aircraft, List<int> naval) CategorizeVariants(
            IReadOnlyList<VariantData> allVariants)
        {
            var buildingVariants = new List<int>();
            var aircraftVariants = new List<int>();
            var navalVariants = new List<int>();
            var processedVariants = new HashSet<int>();

            try
            {
                // Group variants by parent and category
                var variantGroups = allVariants
                    .Where(v => _variantParentCache.ContainsKey(v.Index))
                    .GroupBy(v => _variantParentCache[v.Index])
                    .Where(g => !string.IsNullOrEmpty(g.Key))
                    .ToList();

                // First, categorize all variants
                foreach (var group in variantGroups)
                {
                    var firstVariant = group.First();
                    var variants = SortVariantGroup(group);

                    // Use our existing category detection
                    if (IsAircraftVariant(firstVariant.Index))
                    {
                        aircraftVariants.AddRange(
                            variants.Where(v => processedVariants.Add(v)));
                    }
                    else if (IsNavalVariant(firstVariant.Index))
                    {
                        navalVariants.AddRange(
                            variants.Where(v => processedVariants.Add(v)));
                    }
                    else
                    {
                        buildingVariants.AddRange(
                            variants.Where(v => processedVariants.Add(v)));
                    }
                }

                // Then sort each category according to StaticPrefabKeys order
                buildingVariants = ReorderVariantsByPrefabKeys(
                    buildingVariants,
                    StaticPrefabKeys.Buildings.AllKeys.OfType<BuildingKey>());

                aircraftVariants = ReorderVariantsByPrefabKeys(
                    aircraftVariants,
                    StaticPrefabKeys.Units.AllKeys
                        .OfType<UnitKey>()
                        .Where(k => k.UnitCategory == UnitCategory.Aircraft));

                navalVariants = ReorderVariantsByPrefabKeys(
                    navalVariants,
                    StaticPrefabKeys.Units.AllKeys
                        .OfType<UnitKey>()
                        .Where(k => k.UnitCategory == UnitCategory.Naval));

#if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.Log($"[VariantSorter] Processed variants by category:" +
                         $"\n  Buildings: {buildingVariants.Count}" +
                         $"\n  Aircraft: {aircraftVariants.Count}" +
                         $"\n  Naval: {navalVariants.Count}" +
                         $"\n  Total processed: {processedVariants.Count}" +
                         $"\n  Input variants: {allVariants.Count}");
#endif

                if (processedVariants.Count != allVariants.Count)
                {
                    Debug.LogWarning($"[VariantSorter] Variant count mismatch! " +
                                   $"Processed {processedVariants.Count} vs Input {allVariants.Count}");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[VariantSorter] Error categorizing variants: {e}");
            }

            return (buildingVariants, aircraftVariants, navalVariants);
        }

        private List<int> ReorderVariantsByPrefabKeys<T>(List<int> variants, IEnumerable<T> orderedKeys)
            where T : IPrefabKey
        {
            try
            {
                var parentOrder = orderedKeys
                    .Select(k => k.PrefabName.ToLowerInvariant())
                    .ToList();

                return variants
                    .GroupBy(v => _variantParentCache[v])
                    .OrderBy(g =>
                    {
                        var parentName = g.Key.ToLowerInvariant();
                        var index = parentOrder.FindIndex(p => parentName.Contains(p));
                        return index >= 0 ? index : int.MaxValue;
                    })
                    .SelectMany(g => g)
                    .ToList();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[VariantSorter] Error reordering variants: {e}");
                return variants; // Return original list if reordering fails
            }
        }

        // Sorts variants within a group according to their NameKey order.
        // Uses the NameKeyOrder dictionary to determine sort priority.
        private List<int> SortVariantGroup(IGrouping<string, VariantData> group)
        {
            try
            {
                return group
                    .Where(v => v != null && !string.IsNullOrEmpty(v.VariantNameStringKeyBase))
                    .OrderBy(v => NameKeyOrder.TryGetValue(v.VariantNameStringKeyBase, out int order) ? order : 999)
                    .Select(v => v.Index)
                    .ToList();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[VariantSorter] Error sorting variant group: {e}");
                return new List<int>();
            }
        }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        private void LogExampleVariant(VariantData variant)
        {
            Debug.Log($"[VariantSorter] Example variant {variant.Index}:" +
                     $"\n  PrefabName: {variant.VariantPrefabName}" +
                     $"\n  Credits: {variant.VariantCredits}" +
                     $"\n  Coins: {variant.VariantCoins}" +
                     $"\n  NameKey: {variant.VariantNameStringKeyBase}");
        }
#endif

        // Helper methods for category detection
        private bool IsAircraftVariant(int variantIndex) =>
            IsVariantOfCategory(variantIndex, UnitCategory.Aircraft);

        private bool IsNavalVariant(int variantIndex) =>
            IsVariantOfCategory(variantIndex, UnitCategory.Naval);

        private bool IsVariantOfCategory(int variantIndex, UnitCategory category) =>
            _variantParentCache.TryGetValue(variantIndex, out string parentName) &&
            DataProvider.GameModel.UnlockedUnits
                .Where(u => u.UnitCategory == category)
                .Any(u => parentName.Contains(u.PrefabName.ToLowerInvariant()));

        private void LogUnitData(UnitKey unit)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"=== Unit {unit.PrefabName} Properties ===");

            // Known properties
            sb.AppendLine($"PrefabName: {unit.PrefabName}");
            sb.AppendLine($"UnitCategory: {unit.UnitCategory}");

            // Dynamic properties
            var properties = unit.GetType().GetProperties();
            foreach (var prop in properties)
            {
                try
                {
                    var value = prop.GetValue(unit);
                    sb.AppendLine($"{prop.Name}: {value}");
                }
                catch (System.Exception e)
                {
                    sb.AppendLine($"{prop.Name}: <error: {e.Message}>");
                }
            }
            sb.AppendLine("=== End Unit Properties ===");

            // Log everything at once
            Debug.Log(sb.ToString());
        }

        private void LogVariantData(VariantData variant)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"=== Variant {variant.Index} Properties ===");

            // Known properties
            sb.AppendLine($"Index: {variant.Index}");

            // Dynamic properties
            var properties = variant.GetType().GetProperties();
            foreach (var prop in properties)
            {
                try
                {
                    var value = prop.GetValue(variant);
                    sb.AppendLine($"{prop.Name}: {value}");
                }
                catch (System.Exception e)
                {
                    sb.AppendLine($"{prop.Name}: <error: {e.Message}>");
                }
            }
            sb.AppendLine("=== End Variant Properties ===");

            // Log everything at once with LogWarning
            Debug.LogWarning(sb.ToString());
        }
    }  // end of VariantSorter class
}  // end of namespace
