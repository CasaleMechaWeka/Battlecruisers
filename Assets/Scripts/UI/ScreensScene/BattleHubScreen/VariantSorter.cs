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
    // Advanced variant organizer that sorts units by category and type.
    // - Groups variants by their parent unit
    // - Sorts groups by category (Buildings -> Aircraft -> Naval)
    // - Orders variants within groups by their type (QuickBuild, RapidFire, etc.)
    public class VariantSorter
    {
        private readonly IDataProvider _dataProvider;
        private readonly IPrefabFactory _prefabFactory;
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

        public VariantSorter(IDataProvider dataProvider, IPrefabFactory prefabFactory)
        {
            _dataProvider = dataProvider ?? throw new System.ArgumentNullException(nameof(dataProvider));
            _prefabFactory = prefabFactory ?? throw new System.ArgumentNullException(nameof(prefabFactory));
            _variantParentCache = new Dictionary<int, string>();
        }

        // Returns a list of variant indices organized by category and type.
        // Buildings are listed first, followed by Aircraft, then Naval units.
        // Within each parent group, variants are sorted by their NameKey order.
        public List<int> GetOrganizedVariants()
        {
            try
            {
                var allVariants = _dataProvider.StaticData.Variants;
                if (allVariants == null || allVariants.Count == 0)
                {
                    Debug.LogWarning("[VariantSorter] No variants found");
                    return new List<int>();
                }

                #if UNITY_EDITOR || DEVELOPMENT_BUILD
                LogExampleVariant(allVariants[0]);
                #endif

                CacheVariantParents(allVariants);
                
                Debug.Log($"[VariantSorter] Found {allVariants.Count} total variants");
                
                var uncachedVariants = allVariants.Where(v => !_variantParentCache.ContainsKey(v.Index)).ToList();
                if (uncachedVariants.Any())
                {
                    Debug.LogWarning($"[VariantSorter] Found {uncachedVariants.Count} variants without parents");
                }

                var (buildingVariants, aircraftVariants, navalVariants) = CategorizeVariants(allVariants);

                var result = buildingVariants
                    .Concat(aircraftVariants)
                    .Concat(navalVariants)
                    .ToList();

                #if UNITY_EDITOR || DEVELOPMENT_BUILD
                var parentCounts = result.GroupBy(v => _variantParentCache[v])
                    .OrderBy(g => g.Key)
                    .ToDictionary(g => g.Key, g => g.Count());

                var sb = new StringBuilder();
                sb.AppendLine($"[VariantSorter] Returning {result.Count} organized variants:");
                sb.AppendLine("Parent groups:");
                foreach (var kvp in parentCounts)
                {
                    sb.AppendLine($"  {kvp.Key}: {kvp.Value} variants");
                }
                Debug.Log(sb.ToString());
                #endif

                if (result.Count < allVariants.Count - uncachedVariants.Count)
                {
                    Debug.LogWarning($"[VariantSorter] Lost {allVariants.Count - uncachedVariants.Count - result.Count} variants during organization");
                }

                return result;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[VariantSorter] Critical error: {e}");
                return _dataProvider.StaticData.Variants?
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
                        var prefab = _prefabFactory.GetVariant(
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

        private (List<int> building, List<int> aircraft, List<int> naval) CategorizeVariants(
            IReadOnlyList<VariantData> allVariants)
        {
            var buildingVariants = new List<int>();
            var aircraftVariants = new List<int>();
            var navalVariants = new List<int>();
            var processedVariants = new HashSet<int>();

            try 
            {
                var variantGroups = allVariants
                    .Where(v => _variantParentCache.ContainsKey(v.Index))
                    .GroupBy(v => _variantParentCache[v.Index])
                    .Where(g => !string.IsNullOrEmpty(g.Key))
                    .ToList();

                foreach (var group in variantGroups)
                {
                    var firstVariant = group.First();
                    var variants = SortVariantGroup(group);

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
                    .OrderBy(g => {
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
                return variants;
            }
        }

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

        private bool IsAircraftVariant(int variantIndex) => 
            IsVariantOfCategory(variantIndex, UnitCategory.Aircraft);

        private bool IsNavalVariant(int variantIndex) => 
            IsVariantOfCategory(variantIndex, UnitCategory.Naval);

        private bool IsVariantOfCategory(int variantIndex, UnitCategory category) =>
            _variantParentCache.TryGetValue(variantIndex, out string parentName) &&
            _dataProvider.GameModel.UnlockedUnits
                .Where(u => u.UnitCategory == category)
                .Any(u => parentName.Contains(u.PrefabName.ToLowerInvariant()));
    }
} 