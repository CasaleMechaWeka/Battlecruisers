using BattleCruisers.Data.Models.PrefabKeys;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Static.LevelLoot;
using BattleCruisers.UI.ScreensScene.PostBattleScreen;
using BattleCruisers.Scenes;
using Newtonsoft.Json;

// Remember, this class is going to be fed into a JSON (de)serializer!
// Keep data types primitive.
// JSON has never heard of Battlecruisers, so it gets confused by our internal structures.

namespace BattleCruisers.Data.Models
{
    [Serializable]
    public class SaveGameModel
    {
        // Support both underscore-prefixed (legacy) and non-prefixed (current) JSON formats
        [JsonProperty(PropertyName = "_saveVersion", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int saveVersion;

        // What do we need to save, critically? Just the assets and progress.

        // Total historic destruction score.
        [JsonProperty(PropertyName = "_lifetimeDestructionScore", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long lifetimeDestructionScore;
        
        [JsonProperty(PropertyName = "_battleWinScore", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public float battleWinScore;

        // Number of times lost in the most recent level
        [JsonProperty(PropertyName = "_timesLostOnLastLevel", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int timesLostOnLastLevel;
        
        [JsonProperty(PropertyName = "_bounty", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int bounty;

        // My callsign.
        [JsonProperty(PropertyName = "_playerName", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string playerName;

        // My selected loadout.
        // Selected Captain.
        // Loadout data confuses the serializer! We must map it by hand.
        public string currentHullKey;
        public Dictionary<string, string> currentBuildings;
        public Dictionary<string, string> currentUnits;
        public List<int> currentHeckles;
        public string currentCaptain;
        public Dictionary<string, Dictionary<string, string>> buildingsToCategories;
        public Dictionary<string, Dictionary<string, string>> unitsToCategories;
        public int currentBodykit;
        public List<int> selectedVariants;

        // What levels have been completed
        // What difficulty those levels have been completed at.
        public Dictionary<int, int> levelsCompleted;                          // int levelNum, enum (as int) hardestDifficulty
        public Dictionary<int, int> sideQuestsCompleted;                          // int SideQuest, enum (as int) hardestDifficulty

        // Assets unlocked
        public List<string> unlockedHulls;                            // prefab filenames
        public Dictionary<string, string> unlockedBuildings;          // prefab filenames, category enum strings
        public Dictionary<string, string> unlockedUnits;              // prefab filenames, category enum strings

        // IAPs
        public List<int> purchasedExos;
        public List<int> purchasedHeckles;
        public List<int> purchasedBodykits;
        public List<int> purchasedVariants;

        // Status tracking
        public bool hasAttemptedTutorial;
        public bool premiumEdition;

        public SaveGameModel()
        { // this is the constructor for cloud load
        }

        // Takes in GameModel, simplifies values where necessary for easier JSON parsing
        public SaveGameModel(GameModel game)
        {

            // ##################################################################################
            // ##################################################################################
            //                     INCREMENT THIS IF YOU CHANGE SAVEGAMEMODEL

            saveVersion = game.SaveVersion;
            // Last change: v5 - Added robust version compatibility system

            // Consider writing handling for loading old saves with mismatched or missing fields.
            // ##################################################################################
            // ##################################################################################


            // GameModel fields:
            lifetimeDestructionScore = game.LifetimeDestructionScore;
            battleWinScore = game.BattleWinScore;
            playerName = game.PlayerName;
            timesLostOnLastLevel = game.TimesLostOnLastLevel;
            bounty = game.Bounty;
            levelsCompleted = ComputeCompletedLevels(game.CompletedLevels);
            sideQuestsCompleted = ComputeCompletedSideQuests(game.CompletedSideQuests);
            unlockedHulls = ComputeUnlockedHulls(game.UnlockedHulls);
            unlockedBuildings = ComputeUnlockedBuildings(game.UnlockedBuildings);
            unlockedUnits = ComputeUnlockedUnits(game.UnlockedUnits);

            // IAPs:
            purchasedExos = game.PurchasedExos.Distinct().ToList();
            purchasedHeckles = game.PurchasedHeckles.Distinct().ToList();
            purchasedBodykits = game.PurchasedBodykits.Distinct().ToList();
            purchasedVariants = game.PurchasedVariants.Distinct().ToList();

            // Loadout fields:
            currentHullKey = game.PlayerLoadout.Hull.PrefabName;
            currentBuildings = ComputeLoadoutBuildings(game.PlayerLoadout);
            currentUnits = ComputeLoadoutUnits(game.PlayerLoadout);
            currentHeckles = game.PlayerLoadout.CurrentHeckles;
            currentCaptain = game.PlayerLoadout.CurrentCaptain.PrefabName;
            buildingsToCategories = ComputeBuildLimits(game.PlayerLoadout.GetBuildLimits());
            unitsToCategories = ComputeUnitLimits(game.PlayerLoadout.GetUnitLimits());
            currentBodykit = game.PlayerLoadout.SelectedBodykit;
            selectedVariants = game.PlayerLoadout.SelectedVariants;

            // Status tracking:
            hasAttemptedTutorial = game.HasAttemptedTutorial;
            premiumEdition = game.PremiumEdition || game.PurchasedBodykits.Contains(0);
        }

        /// <summary>
        /// Gets the normalized save version from Application.version.
        /// Extracts major.minor and converts to save format (e.g., "6.5.107" -> 650, "6.4.0" -> 640)
        /// </summary>
        private static int GetCurrentSaveVersion()
        {
            string version = Application.version;
            string[] parts = version.Split('.');
            if (parts.Length >= 2)
            {
                if (int.TryParse(parts[0], out int major) && int.TryParse(parts[1], out int minor))
                {
                    // Convert major.minor to save version format: 6.5 -> 650, 6.4 -> 640
                    return major * 100 + minor * 10; // 6.5 -> 650, 6.4 -> 640
                }
            }
            // Fallback: use full version conversion if parsing fails
            return ScreensSceneGod.VersionToInt(version);
        }

        /// <summary>
        /// Validates and normalizes save version for cloud save compatibility.
        /// Uses the same logic as Serializer.LoadGame() version handling.
        /// 
        /// Version system:
        /// - Legacy versions (0-5): Old format saves (v3, v4, v5, etc.) - migrate to current version (6.5 = 650)
        /// - Current format (640+): Modern saves using app version numbers
        ///   - 640 = 6.4.0 (in production)
        ///   - 650 = 6.5.0 (current/newest version)
        ///   - All versions >= 640 are compatible with each other
        /// </summary>
        private static int ValidateCloudSaveVersion(int saveVersion)
        {
            int currentVersion = GetCurrentSaveVersion();

            // Legacy versions (0-5): Old format saves (v3, v4, v5, etc.) - migrate to current version (6.5 = 650)
            if (saveVersion >= 0 && saveVersion <= 5)
            {
                Debug.Log($"Cloud save version {saveVersion} is legacy format, migrating to current version {currentVersion}");
                return currentVersion;
            }

            // Current format versions (640+): All modern saves are compatible
            // 640 (6.4.0) and 650 (6.5.0) use the same format, so accept as-is
            if (saveVersion >= 640)
            {
                Debug.Log($"Cloud save version {saveVersion} is compatible with current version {currentVersion} (both use modern format)");
                return saveVersion; // Accept the version as-is (640, 650, etc. are all valid)
            }

            // Invalid versions (between 5 and 640, or negative): migrate to current
            Debug.LogWarning($"Cloud save version {saveVersion} is invalid/unknown. Migrating to current version {currentVersion}");
            return currentVersion;
        }

        // Takes in GameModel, converts and assigns values from SaveGameModel to GameModel
        public void AssignSaveToGameModel(GameModel game)
        {
            // Validate and normalize save version for compatibility (same logic as LoadGame)
            int normalizedVersion = ValidateCloudSaveVersion(saveVersion);
            game.SaveVersion = normalizedVersion;
            Debug.Log($"Assigning save data to GameModel... (original version: {saveVersion}, normalized: {normalizedVersion})");

            if (lifetimeDestructionScore > game.LifetimeDestructionScore)
                game.LifetimeDestructionScore = lifetimeDestructionScore;
                
            Debug.Log($"LifetimeDestructionScore: {game.LifetimeDestructionScore}; Cloud Save value: {lifetimeDestructionScore}");

            if(battleWinScore > game.BattleWinScore)
                game.BattleWinScore = battleWinScore;
            Debug.Log($"BattleWinScore: {game.BattleWinScore}; Cloud Save value: {battleWinScore}");

            if(timesLostOnLastLevel > game.TimesLostOnLastLevel)
                game.TimesLostOnLastLevel = timesLostOnLastLevel;
            Debug.Log($"Times lost on last level: {game.TimesLostOnLastLevel}; Cloud Save value: {timesLostOnLastLevel}");

#if FEATURE_BOUNTY
            if (bounty > game.Bounty)
            {
                game.Bounty = bounty;
                Debug.Log($"Player's bounty: {bounty}");
            }
#endif

            if (game.PlayerName == "Charlie")
            {
                game.PlayerName = playerName;
                Debug.Log($"PlayerName: {playerName}");
            }

            // Log completed levels
            if (levelsCompleted != null)
                foreach (KeyValuePair<int, int> level in levelsCompleted)
                    Debug.Log($"Completed Level: {level.Key}, Difficulty: {level.Value}");

            // Log completed side quests
            if (sideQuestsCompleted != null)
                foreach (KeyValuePair<int, int> sideQuest in sideQuestsCompleted)
                    Debug.Log($"Completed SideQuest: {sideQuest.Key}, Difficulty: {sideQuest.Value}");

            // Log unlocked hulls
            if (unlockedHulls != null)
                foreach (string hull in unlockedHulls)
                    Debug.Log($"Unlocked Hull: {hull}");

            // Log unlocked buildings
            if (unlockedBuildings != null)
                foreach (KeyValuePair<string, string> building in unlockedBuildings)
                    Debug.Log($"Unlocked Building: {building.Key}, Category: {building.Value}");

            // Log unlocked units
            if (unlockedUnits != null)
                foreach (KeyValuePair<string, string> unit in unlockedUnits)
                    Debug.Log($"Unlocked Unit: {unit.Key}, Category: {unit.Value}");

            // IAPs
            // Exos
            if (purchasedExos != null)
            {
                List<int> currentExos = game.PurchasedExos;
                foreach (int exo in purchasedExos)
                    if (!currentExos.Contains(exo))
                        game.AddExo(exo);
            }
            else
                game.AddExo(0);

            // Heckles
            if (purchasedHeckles != null)
            {
                List<int> currentHeckles = game.PurchasedHeckles;
                foreach (int heckle in purchasedHeckles)
                    if (!currentHeckles.Contains(heckle))
                        game.AddHeckle(heckle);
            }
            else if (game.PurchasedHeckles.Count < 3)
            {
                game.AddHeckle(0);
                game.AddHeckle(1);
                game.AddHeckle(2);
            }

            // Bodykits
            if (purchasedBodykits != null)
            {
                Debug.Log("CLOUDBODYKITS: " + purchasedBodykits.Count);
                List<int> currentBodykits = game.PurchasedBodykits;
                Debug.Log("LOCALBODYKITS: " + currentBodykits.Count);
                foreach (int bodykit in purchasedBodykits)
                    if (!currentBodykits.Contains(bodykit))
                        game.AddBodykit(bodykit);
            }

            // Variants
            if (purchasedVariants != null)
            {
                List<int> currentVariants = game.PurchasedVariants;
                foreach (int variant in purchasedVariants)
                    if (!currentVariants.Contains(variant))
                        game.AddVariant(variant);
            }

            // levels completed
            if (levelsCompleted != null)
                foreach (KeyValuePair<int, int> level in levelsCompleted)
                {
                    CompletedLevel cLevel = new CompletedLevel(level.Key, (Settings.Difficulty)level.Value);
                    game.AddCompletedLevel(cLevel);
                    //LootManager.UnlockLevelLoot(level.Value);
                }

            if (sideQuestsCompleted != null)
                foreach (KeyValuePair<int, int> sideQuest in sideQuestsCompleted)
                {
                    CompletedLevel cSideQuest = new CompletedLevel(sideQuest.Key, (Settings.Difficulty)sideQuest.Value);
                    game.AddCompletedSideQuest(cSideQuest);
                    //LootManager.UnlockSideQuestLoot(sideQuest.Value);
                }

            // unlocked hulls
            if (unlockedHulls != null)
                foreach (string hull in unlockedHulls)
                {
                    // Skip null or empty hull strings to prevent invalid HullKey creation
                    if (string.IsNullOrEmpty(hull))
                    {
                        Debug.LogWarning($"Skipping null or empty hull string in unlockedHulls");
                        continue;
                    }
                    HullKey hk = new HullKey(hull);
                    game.AddUnlockedHull(hk);
                }

            // Keys and Vals are reversed for unlocks and current units, because dictionaries require their Keys to be
            // unique. The AddUnlocked methods take an enum as their first arg, which definitionally is not unique.

            // unlocked buildings
            if (unlockedBuildings != null)
                foreach (KeyValuePair<string, string> building in unlockedBuildings)
                {
                    Enum.TryParse(building.Value, out BuildingCategory bc);
                    BuildingKey bk = new BuildingKey(bc, building.Key);
                    game.AddUnlockedBuilding(bk);
                }

            // unlocked units
            if (unlockedUnits != null)
                foreach (KeyValuePair<string, string> unit in unlockedUnits)
                {
                    Enum.TryParse(unit.Value, out UnitCategory uc);
                    UnitKey uk = new UnitKey(uc, unit.Key);
                    game.AddUnlockedUnit(uk);
                }

            // Loadout fields, we create a new loadout from scratch and feed it into the constructor:
            // current hull
            if (string.IsNullOrEmpty(currentHullKey))
                currentHullKey = "Trident"; // Default hull
            HullKey cHull = new HullKey(currentHullKey);

            // current buildings
            List<BuildingKey> buildings = new List<BuildingKey>();
            if (currentBuildings != null)
                foreach (var building in currentBuildings)
                {
                    Enum.TryParse(building.Value, out BuildingCategory bc);
                    BuildingKey bk = new BuildingKey(bc, building.Key);
                    buildings.Add(bk);
                }

            // current units
            List<UnitKey> units = new List<UnitKey>();
            if (currentUnits != null)
                foreach (var unit in currentUnits)
                {
                    Enum.TryParse(unit.Value, out UnitCategory uc);
                    UnitKey uk = new UnitKey(uc, unit.Key);
                    units.Add(uk);
                }

            // selected buildings
            // the data structure here is pretty tough to process.
            Dictionary<BuildingCategory, List<BuildingKey>> buildLimits = new Dictionary<BuildingCategory, List<BuildingKey>>();
            if (buildingsToCategories != null)
                foreach (string buildCat in buildingsToCategories.Keys)
                {
                    Enum.TryParse(buildCat, out BuildingCategory bc);

                    Dictionary<string, string> buildingKeys;
                    buildingsToCategories.TryGetValue(buildCat, out buildingKeys);

                    if (buildingKeys != null)
                    {
                        List<BuildingKey> parsedBuildingKeys = new List<BuildingKey>();
                        foreach (var buildKey in buildingKeys)
                        {
                            Enum.TryParse(buildKey.Value, out BuildingCategory keycat);
                            BuildingKey newKey = new BuildingKey(keycat, buildKey.Key);
                            if (parsedBuildingKeys.Count == 0 || newKey.PrefabName != parsedBuildingKeys[parsedBuildingKeys.Count - 1].PrefabName)
                                parsedBuildingKeys.Add(newKey);
                        }
                        buildLimits.Add(bc, parsedBuildingKeys);
                    }
                }

            // selected units
            // the data structure here is pretty tough to process.
            Dictionary<UnitCategory, List<UnitKey>> unitLimits = new Dictionary<UnitCategory, List<UnitKey>>();
            if (unitsToCategories != null)
                foreach (string unitCat in unitsToCategories.Keys)
                {
                    Enum.TryParse(unitCat, out UnitCategory uc);

                    Dictionary<string, string> unitKeys;
                    unitsToCategories.TryGetValue(unitCat, out unitKeys);

                    if (unitKeys != null)
                    {
                        List<UnitKey> parsedUnitKeys = new List<UnitKey>();
                        foreach (var unitKey in unitKeys)
                        {
                            Enum.TryParse(unitKey.Value, out UnitCategory keycat);
                            UnitKey newKey = new UnitKey(keycat, unitKey.Key);
                            if (parsedUnitKeys.Count == 0 || newKey.PrefabName != parsedUnitKeys[parsedUnitKeys.Count - 1].PrefabName)
                                parsedUnitKeys.Add(newKey);
                        }
                        unitLimits.Add(uc, parsedUnitKeys);
                    }
                }

            // loadout construction actually happens finally:
            game.PlayerLoadout = new Loadout(cHull, buildings, units, buildLimits, unitLimits);

            // current variants
            if (selectedVariants != null)
                game.PlayerLoadout.SelectedVariants = selectedVariants;
            else
                game.PlayerLoadout.SelectedVariants = new List<int>();

            // current bodykit
            if (currentBodykit == -1 || game.PurchasedBodykits.Contains(currentBodykit))
                game.PlayerLoadout.SelectedBodykit = currentBodykit;
            else
                game.PlayerLoadout.SelectedBodykit = -1;

            // current heckles
            if (currentHeckles != null)
            {
                game.PlayerLoadout.CurrentHeckles = currentHeckles;
                foreach (int heckle in currentHeckles)
                    game.AddHeckle(heckle);
            }
            else
            {
                game.PlayerLoadout.CurrentHeckles = ComputeUnlockedHeckles(game);
            }

            // current captain
            if (currentCaptain != null)
                game.PlayerLoadout.CurrentCaptain = new CaptainExoKey(currentCaptain);
            else
                game.PlayerLoadout.CurrentCaptain = new CaptainExoKey("CaptainExo000");

            // Tutorial status check - only add progress, never remove
            if ((levelsCompleted != null && levelsCompleted.ContainsKey(1)) || hasAttemptedTutorial == true)
                game.HasAttemptedTutorial = true;
            // Never set HasAttemptedTutorial to false to avoid overwriting local progress

            game.PremiumEdition = premiumEdition || game.PremiumEdition;
        }

        private Dictionary<int, int> ComputeCompletedLevels(IReadOnlyCollection<CompletedLevel> levels)
        {
            var result = new Dictionary<int, int>();
            if (levels == null)
            {
                Debug.LogWarning("computeCompletedLevels returned null in SaveGameModel");
                return null;
            }
            else
            {
                foreach (var level in levels)
                {
                    result.Add(level.LevelNum, (int)level.HardestDifficulty);
                }
            }
            return result;
        }

        private Dictionary<int, int> ComputeCompletedSideQuests(IReadOnlyCollection<CompletedLevel> sideQuests)
        {
            var result = new Dictionary<int, int>();
            if (sideQuests == null)
            {
                Debug.LogWarning("computeCompletedSideQuests returned null in SaveGameModel");
                return null;
            }
            else
            {
                foreach (var sideQuest in sideQuests)
                {
                    result.Add(sideQuest.LevelNum, (int)sideQuest.HardestDifficulty);
                }
            }
            return result;
        }

        private List<int> ComputeUnlockedHeckles(GameModel game)
        {
            List<int> unlockedHeckles = new List<int>();
            int numHecklesUnlocked = 3;
            while (unlockedHeckles.Count < numHecklesUnlocked)
            {
                int unlockHeckle = UnityEngine.Random.Range(0, 279);
                if (!unlockedHeckles.Contains(unlockHeckle))
                {
                    unlockedHeckles.Add(unlockHeckle);
                }
            }
            return unlockedHeckles;
        }

        private List<string> ComputeUnlockedHulls(IReadOnlyCollection<PrefabKeys.HullKey> hulls)
        {
            var result = new List<string>();
            if (hulls == null)
            {
                Debug.LogWarning("computeUnlockedHulls returned null in SaveGameModel");
                return null;
            }
            else
            {
                foreach (var hull in hulls)
                {
                    result.Add(hull.PrefabName);
                }
            }
            return result;
        }

        private Dictionary<string, string> ComputeUnlockedBuildings(IReadOnlyCollection<PrefabKeys.BuildingKey> buildings)
        {
            var result = new Dictionary<string, string>();
            if (buildings == null)
            {
                Debug.LogWarning("computeUnlockedBuildings returned null in SaveGameModel");
                return null;
            }
            else
            {
                foreach (var building in buildings)
                {
                    string category = building.BuildingCategory.ToString();
                    string prefabName = building.PrefabName;

                    result.Add(prefabName, category);
                }
            }
            return result;
        }

        private Dictionary<string, string> ComputeUnlockedUnits(IReadOnlyCollection<PrefabKeys.UnitKey> units)
        {
            var result = new Dictionary<string, string>();
            if (units == null)
            {
                Debug.LogWarning("computeUnlockedUnits returned null in SaveGameModel");
                return null;
            }
            else
            {
                foreach (var unit in units)
                {
                    string category = unit.UnitCategory.ToString();
                    string prefabName = unit.PrefabName;

                    result.Add(prefabName, category);
                }
            }
            return result;
        }

        private Dictionary<string, string> ComputeLoadoutBuildings(Loadout loadout)
        {
            var result = new Dictionary<string, string>();

            foreach (BuildingKey building in loadout.GetBuildings(BuildingCategory.Factory))
            {
                string category = BuildingCategory.Factory.ToString();
                string prefabName = building.PrefabName;

                result.Add(prefabName, category);
            }

            foreach (BuildingKey building in loadout.GetBuildings(BuildingCategory.Defence))
            {
                string category = BuildingCategory.Defence.ToString();
                string prefabName = building.PrefabName;

                result.Add(prefabName, category);
            }

            foreach (BuildingKey building in loadout.GetBuildings(BuildingCategory.Offence))
            {
                string category = BuildingCategory.Offence.ToString();
                string prefabName = building.PrefabName;

                result.Add(prefabName, category);
            }

            foreach (BuildingKey building in loadout.GetBuildings(BuildingCategory.Tactical))
            {
                string category = BuildingCategory.Tactical.ToString();
                string prefabName = building.PrefabName;

                result.Add(prefabName, category);
            }

            foreach (BuildingKey building in loadout.GetBuildings(BuildingCategory.Ultra))
            {
                string category = BuildingCategory.Ultra.ToString();
                string prefabName = building.PrefabName;

                result.Add(prefabName, category);
            }

            return result;
        }

        private Dictionary<string, string> ComputeLoadoutUnits(Loadout loadout)
        {
            var result = new Dictionary<string, string>();

            foreach (UnitKey unit in loadout.GetUnits(UnitCategory.Aircraft))
            {
                string category = UnitCategory.Aircraft.ToString();
                string prefabName = unit.PrefabName;

                result.Add(prefabName, category);
            }

            foreach (UnitKey unit in loadout.GetUnits(UnitCategory.Naval))
            {
                string category = UnitCategory.Naval.ToString();
                string prefabName = unit.PrefabName;

                result.Add(prefabName, category);
            }

            return result;
        }

        private Dictionary<string, Dictionary<string, string>> ComputeBuildLimits(Dictionary<BuildingCategory, List<BuildingKey>> buildLimits)
        {
            var result = new Dictionary<string, Dictionary<string, string>>();

            foreach (BuildingCategory cat in buildLimits.Keys)
            {
                string category = cat.ToString();

                List<BuildingKey> buildings;
                buildLimits.TryGetValue(cat, out buildings);

                Dictionary<string, string> parsedBuildings = new Dictionary<string, string>();
                foreach (BuildingKey unparsedBuilding in buildings)
                {
                    string localCategory = unparsedBuilding.BuildingCategory.ToString();
                    string prefabName = unparsedBuilding.PrefabName;
                    parsedBuildings.Add(prefabName, localCategory);
                }
                result.Add(category, parsedBuildings);
            }

            return result;
        }

        private Dictionary<string, Dictionary<string, string>> ComputeUnitLimits(Dictionary<UnitCategory, List<UnitKey>> unitLimits)
        {
            var result = new Dictionary<string, Dictionary<string, string>>();

            foreach (UnitCategory cat in unitLimits.Keys)
            {
                string category = cat.ToString();

                List<UnitKey> units;
                unitLimits.TryGetValue(cat, out units);

                Dictionary<string, string> parsedUnits = new Dictionary<string, string>();
                foreach (UnitKey unparsedUnit in units)
                {
                    string localCategory = unparsedUnit.UnitCategory.ToString();
                    string prefabName = unparsedUnit.PrefabName;
                    parsedUnits.Add(prefabName, localCategory);
                }
                result.Add(category, parsedUnits);
            }

            return result;
        }
    }
}
