using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using BattleCruisers.Data.Models;
using UnityEngine.Assertions;
using Newtonsoft.Json;
using Unity.Services.CloudSave;
using Unity.Services.Authentication;
using Unity.Services.Core;
using System.Threading.Tasks;
using UnityEngine;
using System;
using Unity.Services.Economy;
using Unity.Services.Economy.Model;
using BattleCruisers.Utils.UGS.Samples;
using BattleCruisers.Data.Static;
using BattleCruisers.Data.Models.PrefabKeys;
using System.Linq;
using System.Reflection;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Scenes;

namespace BattleCruisers.Data
{
    public class Serializer
    {
        private readonly BinaryFormatter _binaryFormatter;

        private readonly string defaultGameModelFilePath;
        private readonly string betaGameModelFilePath;
        private readonly string experimentalGameModelFilePath;
        private readonly string preferredGameModelFilePath;

        public Serializer()
        {
            defaultGameModelFilePath = Application.persistentDataPath + "/GameModel.bcms";
            betaGameModelFilePath = Application.persistentDataPath + "/GameModelBeta.bcms";
            experimentalGameModelFilePath = Application.persistentDataPath + "/GameModelExperimental.bcms";

#if BETA_SAVE
            preferredGameModelFilePath = betaGameModelFilePath;
#elif EXPERIMENTAL_SAVE
            preferredGameModelFilePath = experimentalGameModelFilePath;
#else
            preferredGameModelFilePath = defaultGameModelFilePath;
#endif

            _binaryFormatter = new BinaryFormatter();
        }

        public bool DoesPreferredSaveGameExist()
        {
            return File.Exists(preferredGameModelFilePath);
        }

        public bool DoesSaveGameExist()
        {
#if BETA_SAVE || EXPERIMENTAL_SAVE
            if (DoesPreferredSaveGameExist())
                return File.Exists(preferredGameModelFilePath);
#endif
            return File.Exists(defaultGameModelFilePath);
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

        public void SaveGame(GameModel game)
        {
            game.SaveVersion = GetCurrentSaveVersion();
            using (FileStream file = File.Create(preferredGameModelFilePath))
            {
                _binaryFormatter.Serialize(file, game);
            }
        }

        public GameModel LoadGame()
        {
            // Check if save file exists - if not, go to emergency recovery
            if (!DoesSaveGameExist())
            {
                return EmergencyRecovery();
            }

            string filePath = preferredGameModelFilePath;
#if BETA_SAVE || EXPERIMENTAL_SAVE
            if (!DoesPreferredSaveGameExist())
            {
                if (File.Exists(defaultGameModelFilePath))
                {
                    filePath = defaultGameModelFilePath;
                    Debug.Log("No Beta save file found; defaulting to GameModel.bcms");
                }
            }
#endif

            object output = null;
            try
            {
                using (FileStream file = File.Open(filePath, FileMode.Open))
                {
                    output = _binaryFormatter.Deserialize(file);
                }
            }
            catch
            {
                return EmergencyRecovery();
            }

            if (output == null)
                return EmergencyRecovery();

            int version = GetSaveVersion(output);
            GameModel game;

            // Version compatibility system:
            // - Legacy versions (0-5): Old format saves (v3, v4, v5, etc.) - migrate to current version (6.5 = 650)
            // - Current format (640+): Modern saves using app version numbers
            //   - 640 = 6.4.0 (in production, compatible)
            //   - 650 = 6.5.0 (current/newest version)
            //   - All versions >= 640 use the same format and are compatible with each other
            switch (version)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                    // Legacy formats (v3, v4, v5, etc.) - migrate to current version (6.5 = 650)
                    Debug.Log($"Loading legacy save version {version}, migrating to current version");
                    game = MigrateToCurrentVersion(output);
                    break;

                default:
                    // Modern format versions: 640 (6.4.0), 650 (6.5.0), etc.
                    // All versions >= 640 use the same format and are compatible
                    if (version >= 640)
                    {
                        game = output as GameModel;
                        if (game == null)
                            return EmergencyRecovery();
                        
                        // Version 651 migration: Convert levels 32-40 to sidequests 0-8
                        // This MUST run before ValidateCurrentSave, which rejects saves with >31 completed levels
                        if (version < 651)
                        {
                            MigrateLevels32To40ToSidequests(game);
                            game.SaveVersion = GetCurrentSaveVersion(); // Update to current version after migration
                            SaveGame(game); // Persist the migration
                            Debug.Log($"LoadGame - Migrated save from version {version} to {game.SaveVersion} (levels 32-40 → sidequests)");
                        }
                        
                        if (!ValidateCurrentSave(game))
                            return EmergencyRecovery();
                        
                        Debug.Log($"Loading modern format save version {game.SaveVersion} (compatible with current version)");
                    }
                    else
                    {
                        // Versions between 5 and 640 are invalid/unknown - migrate to current
                        Debug.LogWarning($"Unknown save version {version} (between legacy and modern), migrating to current version");
                        game = MigrateToCurrentVersion(output);
                    }
                    break;
            }

            // Post-load recovery operations (preserve existing logic)
            // If any variant is in SelectedVariants but missing from PurchasedVariants, restore it
            if (game.PlayerLoadout.SelectedVariants != null && game.PlayerLoadout.SelectedVariants.Count > 0)
            {
                int restoredCount = 0;
                foreach (int selectedVariantId in game.PlayerLoadout.SelectedVariants)
                {
                    if (!game.PurchasedVariants.Contains(selectedVariantId))
                    {
                        game.AddVariant(selectedVariantId);
                        restoredCount++;
                        Debug.Log($"RECOVERY: Restored missing purchased variant {selectedVariantId} (found in SelectedVariants)");
                    }
                }

                if (restoredCount > 0)
                    Debug.Log($"RECOVERY: Successfully restored {restoredCount} missing purchased variants from SelectedVariants");
            }

#if PREMIUM_EDITION
            game.PremiumEdition = true;
            game.AddBodykit(0);
#endif

            return game;
        }

        private GameModel MakeCompatible(object gameData)
        {
            Debug.Log("MakeCompatible - Migrating legacy save to current format");

            // perhaps be more conservative for these other fields too?
            var tut = gameData.GetType().GetProperty("HasAttemptedTutorial").GetValue(gameData);
            var lds = gameData.GetType().GetProperty("LifetimeDestructionScore").GetValue(gameData);
            var bds = gameData.GetType().GetProperty("BestDestructionScore").GetValue(gameData);
            var plo = gameData.GetType().GetProperty("PlayerLoadout").GetValue(gameData);
            var lbr = gameData.GetType().GetProperty("LastBattleResult").GetValue(gameData);
            var pre = gameData.GetType().GetProperty("PremiumEdition").GetValue(gameData);
            var sav = gameData.GetType().GetProperty("SaveVersion");
            
            // Get the original save version for logging
            int originalSaveVersion = 0;
            if(sav != null)
                originalSaveVersion = (int)sav.GetValue(gameData);
            
            // Always use current version when migrating legacy saves (0-5, or any version < 640)
            // This ensures old saves (v3, v4, v5, etc.) are migrated to the current version (6.5 = 650)
            int currentVersion = GetCurrentSaveVersion();
            int saveVersion = currentVersion;
            
            Debug.Log($"Migrating save from version {originalSaveVersion} to current version {currentVersion}");

            List<HullKey> _unlockedHulls = new List<HullKey>();
            foreach (var hull in gameData.GetType().GetProperty("UnlockedHulls").GetValue(gameData) as IReadOnlyCollection<HullKey>)
                _unlockedHulls.Add(hull);

            List<BuildingKey> _unlockedBuildings = new List<BuildingKey>();
            foreach (var building in gameData.GetType().GetProperty("UnlockedBuildings").GetValue(gameData) as IReadOnlyCollection<BuildingKey>)
                _unlockedBuildings.Add(building);

            List<UnitKey> _unlockedUnits = new List<UnitKey>();
            foreach (var unit in gameData.GetType().GetProperty("UnlockedUnits").GetValue(gameData) as IReadOnlyCollection<UnitKey>)
                _unlockedUnits.Add(unit);

            // compiler doesn't like them being cast when they're assigned, so they're cast here
            bool _hasAttemptedTutorial = (bool)tut;
            long _lifetimeDestructionScore = (long)lds;
            long _bestDestructionScore = (long)bds;
            Loadout _loadout = (Loadout)plo;
            _loadout.ValidateSelectedBuildables();

            BattleResult _lastBattleResult = (BattleResult)lbr;
            bool _premiumState = (bool)pre;

            bool _hasSyncdShop = false;

            // GameModel gets constructed from the fields we've pulled out of gameData:
            GameModel compatibleGameModel = new GameModel(
                _hasSyncdShop,
                _hasAttemptedTutorial,
                _lifetimeDestructionScore,
                _bestDestructionScore,
                _loadout,
                _lastBattleResult,
                _unlockedHulls,
                _unlockedBuildings,
                _unlockedUnits,
                saveVersion
                );

            compatibleGameModel.PremiumEdition = _premiumState;

            if (_loadout.CurrentCaptain != null)
            {
                string captainName = _loadout.CurrentCaptain.PrefabName;
                if (captainName.StartsWith("CaptainExo"))
                {
                    string indexStr = captainName.Replace("CaptainExo", "");
                    if (int.TryParse(indexStr, out int captainIndex))
                    {
                        compatibleGameModel.AddExo(captainIndex);
                    }
                }
            }
            else
            {
                compatibleGameModel.PlayerLoadout.PurchaseExo("CaptainExo000");
                compatibleGameModel.PlayerLoadout.CurrentCaptain = new CaptainExoKey("CaptainExo000");
                compatibleGameModel.AddExo(0);
            }

            // Extract CurrentHeckles and ensure they're in purchased Heckles
            if (_loadout.CurrentHeckles != null && _loadout.CurrentHeckles.Count > 0)
            {
                foreach (int heckleId in _loadout.CurrentHeckles)
                {
                    compatibleGameModel.AddHeckle(heckleId);
                }
            }
            else
            {
                compatibleGameModel.PlayerLoadout.CurrentHeckles = new List<int> { 0, 1, 2 };
            }

            PropertyInfo[] properties = gameData.GetType().GetProperties();
            //foreach (PropertyInfo propertyInfo in properties)
            //    Debug.Log(propertyInfo.Name);

            string[] purchasableCategories = new string[]
            {
                "Heckles", "Exos", "Bodykits", "Variants"
            };

            Action<int>[] purchasableOperations = new Action<int>[]
            {
                compatibleGameModel.AddHeckle,
                compatibleGameModel.AddExo,
                compatibleGameModel.AddBodykit,
                compatibleGameModel.AddVariant
            };

            for (int i = 0; i < purchasableCategories.Length; i++)
                try
                {
                    var purchasableProperty = gameData.GetType().GetProperty("Purchased" + purchasableCategories[i]);
                    if (purchasableProperty?.GetValue(gameData) is List<int> purchasableItems && purchasableItems.Count > 0)
                        foreach (int j in purchasableItems)
                            purchasableOperations[i](j);
                }
                catch { }

            string[] purchasableCategoriesLegacy = new string[]
            {
                "Heckles", "Captains", "Bodykits", "Variants"
            };

            for (int i = 0; i < purchasableCategoriesLegacy.Length; i++)
                try
                {
                    var purchasableProperty = gameData.GetType().GetProperty(purchasableCategoriesLegacy[i]);
                    if (purchasableProperty?.GetValue(gameData) is List<int> purchasableItems && purchasableItems.Count > 0)
                        foreach (int? j in purchasableItems)
                        {
                            if (j == null) continue;
                            var isOwnedProperty = j.GetType().GetProperty("isOwned");
                            if (isOwnedProperty == null) continue;
                            var indexProperty = j.GetType().GetProperty("index");
                            if (indexProperty == null) continue;
                            if (isOwnedProperty.GetValue(j) is bool isOwned && isOwned
                            && indexProperty.GetValue(j) is int index)
                                purchasableOperations[i](index);
                        }
                }
                catch { }

            if (gameData.GetType().GetProperty("BattleWinScore").GetValue(gameData) != null)
                compatibleGameModel.BattleWinScore = (float)gameData.GetType().GetProperty("BattleWinScore").GetValue(gameData);

            if (gameData.GetType().GetProperty("Coins").GetValue(gameData) != null)
                compatibleGameModel.Coins = (long)gameData.GetType().GetProperty("Coins").GetValue(gameData);

            if (gameData.GetType().GetProperty("Credits").GetValue(gameData) != null)
                compatibleGameModel.Credits = (long)gameData.GetType().GetProperty("Credits").GetValue(gameData);

            if (gameData.GetType().GetProperty("CoinsChange").GetValue(gameData) != null)
                compatibleGameModel.CoinsChange = (int)gameData.GetType().GetProperty("CoinsChange").GetValue(gameData);

            if (gameData.GetType().GetProperty("CreditsChange").GetValue(gameData) != null)
                compatibleGameModel.CreditsChange = (int)gameData.GetType().GetProperty("CreditsChange").GetValue(gameData);

            if (gameData.GetType().GetProperty("TimesLostOnLastLevel").GetValue(gameData) != null)
            {
                compatibleGameModel.TimesLostOnLastLevel = (int)gameData.GetType().GetProperty("TimesLostOnLastLevel").GetValue(gameData);
            }

            if (gameData.GetType().GetProperty("Bounty").GetValue(gameData) != null)
                compatibleGameModel.Bounty = (int)gameData.GetType().GetProperty("Bounty").GetValue(gameData);

            // Extract SelectedBodykit if it exists
            var bodykitField = _loadout.GetType().GetField("_selectedBodykit", BindingFlags.NonPublic | BindingFlags.Instance);
            if (bodykitField != null)
            {
                var bodykitValue = bodykitField.GetValue(_loadout);
                if (bodykitValue is int bodykitIndex && bodykitIndex >= 0)
                {
                    compatibleGameModel.AddBodykit(bodykitIndex);
                }
            }

            // Extract SelectedVariants and ensure they're in purchased Variants
            if (_loadout.SelectedVariants == null)
                _loadout.SelectedVariants = new List<int>();
            
            foreach (int variantId in _loadout.SelectedVariants)
            {
                compatibleGameModel.AddVariant(variantId);
            }

            // Player Name
            string _playerName = gameData.GetType().GetProperty("PlayerName").GetValue(gameData) as string;
            if (_playerName == null || _playerName == "")
            {
                compatibleGameModel.PlayerName = "Charlie";
            }
            else
            {
                compatibleGameModel.PlayerName = _playerName;
            }

            IReadOnlyCollection<CompletedLevel> completedLevels = gameData.GetType().GetProperty("CompletedLevels").GetValue(gameData) as IReadOnlyCollection<CompletedLevel>;

            // Add completed levels 1-31 only (levels 32-40 will be migrated to sidequests)
            foreach (var cl in completedLevels)
            {
                if (cl.LevelNum >= 1 && cl.LevelNum <= StaticData.NUM_OF_LEVELS)
                {
                    compatibleGameModel.AddCompletedLevel(cl);
                }
            }

            // Migrate levels 32-40 to sidequests 0-8
            // NOTE: Sidequests 0-8 may already be complete from the existing sidequest logic below
            // So we need to handle: level complete + sidequest not complete, level complete + sidequest complete
            var levelsToMigrate = completedLevels.Where(cl => cl.LevelNum >= 32 && cl.LevelNum <= 40).ToList();

            if (levelsToMigrate.Count > 0)
            {
                Debug.Log($"MakeCompatible - Found {levelsToMigrate.Count} old levels (32-40) to migrate to sidequests");
                
                // We'll process these AFTER the existing sidequest unlock logic runs below
                // Store them for later processing
                foreach (var cl in levelsToMigrate)
                {
                    int sideQuestId = cl.LevelNum - 32; // 32→0, 33→1, ..., 40→8
                    
                    // Check if already marked complete by the unlock-based logic below
                    // (This will be checked again after that logic runs)
                    // For now, just log what we found
                    Debug.Log($"MakeCompatible - Will migrate level {cl.LevelNum} to sidequest {sideQuestId} (difficulty: {cl.HardestDifficulty})");
                }
            }

            List<int> completedSideQuestIDs = compatibleGameModel.CompletedSideQuests.Select(data => data.LevelNum).ToList();

            //needs to be hardcoded since otherwise access to StaticData.cs would be required
            //update this whenever loot unlock requirements are modified

            if (_unlockedBuildings.Contains(StaticPrefabKeys.Buildings.NovaArtillery) && !completedSideQuestIDs.Contains(0))
                compatibleGameModel.AddCompletedSideQuest(new CompletedLevel(0, Settings.Difficulty.Hard));
            if (_unlockedHulls.Contains(StaticPrefabKeys.Hulls.Rickshaw) && !completedSideQuestIDs.Contains(1))
                compatibleGameModel.AddCompletedSideQuest(new CompletedLevel(1, Settings.Difficulty.Hard));
            if (_unlockedHulls.Contains(StaticPrefabKeys.Hulls.TasDevil) && !completedSideQuestIDs.Contains(2))
                compatibleGameModel.AddCompletedSideQuest(new CompletedLevel(2, Settings.Difficulty.Hard));
            if (_unlockedBuildings.Contains(StaticPrefabKeys.Buildings.MissilePod) && !completedSideQuestIDs.Contains(3))
                compatibleGameModel.AddCompletedSideQuest(new CompletedLevel(3, Settings.Difficulty.Hard));
            if (_unlockedHulls.Contains(StaticPrefabKeys.Hulls.BlackRig) && !completedSideQuestIDs.Contains(4))
                compatibleGameModel.AddCompletedSideQuest(new CompletedLevel(4, Settings.Difficulty.Hard));
            if (_unlockedBuildings.Contains(StaticPrefabKeys.Buildings.IonCannon) && !completedSideQuestIDs.Contains(5))
                compatibleGameModel.AddCompletedSideQuest(new CompletedLevel(5, Settings.Difficulty.Hard));
            if (_unlockedBuildings.Contains(StaticPrefabKeys.Buildings.Coastguard) && !completedSideQuestIDs.Contains(6))
                compatibleGameModel.AddCompletedSideQuest(new CompletedLevel(6, Settings.Difficulty.Hard));
            if (_unlockedHulls.Contains(StaticPrefabKeys.Hulls.Yeti) && !completedSideQuestIDs.Contains(7))
                compatibleGameModel.AddCompletedSideQuest(new CompletedLevel(7, Settings.Difficulty.Hard));
            if (_unlockedUnits.Contains(StaticPrefabKeys.Units.Broadsword) && !completedSideQuestIDs.Contains(8))
                compatibleGameModel.AddCompletedSideQuest(new CompletedLevel(8, Settings.Difficulty.Hard));

            // NOW migrate levels 32-40 to sidequests, handling cases where sidequests may already be complete
            foreach (var cl in completedLevels.Where(c => c.LevelNum >= 32 && c.LevelNum <= 40))
            {
                int sideQuestId = cl.LevelNum - 32;
                
                if (!completedSideQuestIDs.Contains(sideQuestId))
                {
                    // Sidequest not yet complete - mark it complete from level completion
                    compatibleGameModel.AddCompletedSideQuest(new CompletedLevel(sideQuestId, cl.HardestDifficulty));
                    Debug.Log($"MakeCompatible - Migrated level {cl.LevelNum} → sidequest {sideQuestId} (NEW)");
                }
                else
                {
                    // Sidequest already complete - update difficulty if level had higher difficulty
                    var sideQuestField = typeof(GameModel).GetField("_completedSideQuests", BindingFlags.NonPublic | BindingFlags.Instance);
                    var completedSideQuests = sideQuestField.GetValue(compatibleGameModel) as List<CompletedLevel>;
                    if (completedSideQuests != null)
                    {
                        var existingSideQuest = completedSideQuests.FirstOrDefault(sq => sq.LevelNum == sideQuestId);
                        if (existingSideQuest != null && cl.HardestDifficulty > existingSideQuest.HardestDifficulty)
                        {
                            existingSideQuest.HardestDifficulty = cl.HardestDifficulty;
                            Debug.Log($"MakeCompatible - Level {cl.LevelNum} → sidequest {sideQuestId} (UPDATED difficulty to {cl.HardestDifficulty})");
                        }
                        else
                        {
                            Debug.Log($"MakeCompatible - Level {cl.LevelNum} → sidequest {sideQuestId} (no update needed, sidequest difficulty is higher or equal)");
                        }
                    }
                }
            }

            // Refresh the completedSideQuestIDs list after migrations
            completedSideQuestIDs = compatibleGameModel.CompletedSideQuests.Select(data => data.LevelNum).ToList();

            if (compatibleGameModel.CompletedLevels != null && compatibleGameModel.CompletedLevels.Count > 0)
                compatibleGameModel.HasAttemptedTutorial = true;

            return compatibleGameModel;
        }

        private int GetSaveVersion(object gameData)
        {
            if (gameData is GameModel gameModel)
                return gameModel.SaveVersion;

            var prop = gameData.GetType().GetProperty("SaveVersion");
            if (prop != null && prop.GetValue(gameData) is int version)
                return version;

            return 0;
        }

        private bool ValidateCurrentSave(GameModel game)
        {
            if (game == null)
                return false;

            // Check critical loadout properties
            if (game.PlayerLoadout == null)
                return false;

            if (game.PlayerLoadout.CurrentCaptain == null)
                return false;

            if (game.PlayerLoadout.SelectedVariants == null)
                return false;

            // Check Purchased lists exist and are correct type
            string[] purchasableCategories = new string[] { "Heckles", "Exos", "Bodykits", "Variants" };
            foreach (string category in purchasableCategories)
            {
                try
                {
                    var prop = game.GetType().GetProperty("Purchased" + category);
                    if (prop == null)
                        return false;

                    var value = prop.GetValue(game);
                    if (!(value is List<int>))
                        return false;
                }
                catch
                {
                    return false;
                }
            }

            // Check loadout category counts
            try
            {
                if (game.PlayerLoadout.SelectedBuildings[BuildingCategory.Factory].Count > 5 ||
                   game.PlayerLoadout.SelectedBuildings[BuildingCategory.Defence].Count > 5 ||
                   game.PlayerLoadout.SelectedBuildings[BuildingCategory.Offence].Count > 5 ||
                   game.PlayerLoadout.SelectedBuildings[BuildingCategory.Ultra].Count > 5 ||
                   game.PlayerLoadout.SelectedUnits[UnitCategory.Naval].Count > 5 ||
                   game.PlayerLoadout.SelectedUnits[UnitCategory.Aircraft].Count > 5)
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

            // Check completed levels count
            if (game.NumOfLevelsCompleted > StaticData.NUM_OF_LEVELS)
                return false;

            // Try ValidateSelectedBuildables
            try
            {
                game.PlayerLoadout.ValidateSelectedBuildables();
            }
            catch
            {
                return false;
            }

            return true;
        }

        private GameModel MigrateToCurrentVersion(object gameData)
        {
            GameModel game = MakeCompatible(gameData);
            game.SaveVersion = GetCurrentSaveVersion();
            SaveGame(game);
            return game;
        }

        /// <summary>
        /// Migrates completed levels 32-40 to sidequests 0-8.
        /// Called for saves created before version 651 when levels 32-40 were converted to sidequests.
        /// Mapping: Level 32 → Sidequest 0, Level 33 → Sidequest 1, ..., Level 40 → Sidequest 8
        /// 
        /// IMPORTANT: Sidequests 0-8 may already be complete independently (they existed before this migration).
        /// This method handles all combinations:
        /// - Level complete, sidequest not complete → Mark sidequest complete
        /// - Level complete, sidequest complete → Keep higher difficulty
        /// - Level not complete, sidequest complete → No change needed
        /// - Neither complete → No change needed
        /// 
        /// In ALL cases where a level 32-40 exists, it is removed from CompletedLevels.
        /// </summary>
        private void MigrateLevels32To40ToSidequests(GameModel game)
        {
            Debug.Log("MigrateLevels32To40ToSidequests - Checking for levels 32-40 to migrate");
            
            // Find completed levels in range 32-40
            var levelsToMigrate = game.CompletedLevels
                .Where(cl => cl.LevelNum >= 32 && cl.LevelNum <= 40)
                .ToList();
            
            if (levelsToMigrate.Count == 0)
            {
                Debug.Log("MigrateLevels32To40ToSidequests - No levels 32-40 found to migrate");
                return;
            }
            
            Debug.Log($"MigrateLevels32To40ToSidequests - Found {levelsToMigrate.Count} completed levels (32-40) to process");
            
            foreach (var completedLevel in levelsToMigrate)
            {
                // Map: Level 32 → Sidequest 0, Level 33 → Sidequest 1, etc.
                int sideQuestId = completedLevel.LevelNum - 32;
                
                // Check if sidequest is already complete
                bool sideQuestAlreadyComplete = game.IsSideQuestCompleted(sideQuestId);
                
                if (!sideQuestAlreadyComplete)
                {
                    // Scenario B: Level complete, sidequest not complete → Mark sidequest complete
                    game.AddCompletedSideQuest(new CompletedLevel(sideQuestId, completedLevel.HardestDifficulty));
                    Debug.Log($"MigrateLevels32To40ToSidequests - Level {completedLevel.LevelNum} → Sidequest {sideQuestId} (NEW, difficulty: {completedLevel.HardestDifficulty})");
                }
                else
                {
                    // Scenario D: Both complete → Keep higher difficulty
                    var sideQuestField = typeof(GameModel).GetField("_completedSideQuests", BindingFlags.NonPublic | BindingFlags.Instance);
                    var completedSideQuests = sideQuestField.GetValue(game) as List<CompletedLevel>;
                    if (completedSideQuests != null)
                    {
                        var existingSideQuest = completedSideQuests.FirstOrDefault(sq => sq.LevelNum == sideQuestId);
                        if (existingSideQuest != null && completedLevel.HardestDifficulty > existingSideQuest.HardestDifficulty)
                        {
                            existingSideQuest.HardestDifficulty = completedLevel.HardestDifficulty;
                            Debug.Log($"MigrateLevels32To40ToSidequests - Level {completedLevel.LevelNum} → Sidequest {sideQuestId} (UPDATED difficulty to {completedLevel.HardestDifficulty})");
                        }
                        else
                        {
                            Debug.Log($"MigrateLevels32To40ToSidequests - Level {completedLevel.LevelNum} → Sidequest {sideQuestId} (no update needed, sidequest difficulty is higher or equal)");
                        }
                    }
                }
                
                // ALWAYS remove the level from CompletedLevels (makes room for new levels 32-40)
                game.RemoveCompletedLevel(completedLevel.LevelNum);
                Debug.Log($"MigrateLevels32To40ToSidequests - Removed level {completedLevel.LevelNum} from CompletedLevels");
            }
            
            Debug.Log($"MigrateLevels32To40ToSidequests - Migration complete. Campaign levels: {game.NumOfLevelsCompleted}, Sidequests: {game.NumOfSideQuestsCompleted}");
        }

        private GameModel BuildMinimalDefaults()
        {
            return StaticData.InitialGameModel;
        }

        private GameModel EmergencyRecovery()
        {
            GameModel minimal = BuildMinimalDefaults();
            minimal.SaveVersion = GetCurrentSaveVersion();
            SaveGame(minimal);
            return minimal;
        }

        public void DeleteSavedGame()
        {
            if (DoesPreferredSaveGameExist())
            {
                File.Delete(preferredGameModelFilePath);
            }
        }

        public object DeserializeGameModel(string gameModelJSON)
        {
            // Configure to handle both underscore-prefixed (legacy) and non-prefixed (current) formats
            var settings = new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            return JsonConvert.DeserializeObject<SaveGameModel>(gameModelJSON, settings);
        }

        public string SerializeGameModel(object saveGameModel)
        {
            return JsonConvert.SerializeObject(saveGameModel); //, new JsonSerializerSettings
            //{
            //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //});
            // ^ uncomment this if serializing vectors
        }

        public async Task<bool> CloudSave(GameModel game)
        {
            try
            {
                // CRITICAL: Verify authentication state right before attempting save
                // This prevents race conditions where sign-out happens between checks
                if (Unity.Services.Core.UnityServices.State != Unity.Services.Core.ServicesInitializationState.Initialized)
                {
                    Debug.LogWarning("CloudSave: Unity Services not initialized");
                    return false;
                }

                if (Unity.Services.Authentication.AuthenticationService.Instance == null)
                {
                    Debug.LogWarning("CloudSave: AuthenticationService.Instance is null");
                    return false;
                }

                if (!Unity.Services.Authentication.AuthenticationService.Instance.IsSignedIn)
                {
                    Debug.LogWarning("CloudSave: User is not signed in - cannot save to cloud");
                    return false;
                }

                game.SaveVersion = GetCurrentSaveVersion();
                SaveGameModel saveData = new SaveGameModel(game);
                if (CloudSaveService.Instance != null && CloudSaveService.Instance.Data != null)
                {
                    var serializedData = SerializeGameModel(saveData);
                    if (!string.IsNullOrEmpty(serializedData))
                    {
                        var data = new Dictionary<string, object> { { "GameModel", serializedData } };
                        
                        // Double-check authentication right before the actual save call
                        if (!Unity.Services.Authentication.AuthenticationService.Instance.IsSignedIn)
                        {
                            Debug.LogWarning("CloudSave: User signed out during save operation");
                            return false;
                        }
                        
                        await CloudSaveService.Instance.Data.ForceSaveAsync(data);
                        Debug.Log($"[Serializer] CloudSave: Successfully saved to cloud (PlayerId: {Unity.Services.Authentication.AuthenticationService.Instance.PlayerId})");
                        return true; // Success
                    }
                    else
                    {
                        Debug.LogError("CloudSave Error: Serialized data is empty or null.");
                        return false;
                    }
                }
                else
                {
                    Debug.LogError("CloudSave Error: CloudSaveService instance or Data is null.");
                    return false;
                }
            }
            catch (TimeoutException e)
            {
                Debug.LogWarning($"CloudSave Timeout occurred: {e.Message}");
                return false;
            }
            catch (Unity.Services.Authentication.AuthenticationException e)
            {
                Debug.LogError($"CloudSave Authentication Error: {e.Message}");
                return false;
            }
            catch (Exception e)
            {
                Debug.LogError($"CloudSave Error: {e.GetType().Name} - {e.Message}");
                return false;
            }
        }

        public async Task<SaveGameModel> CloudLoad(GameModel game)
        {
            try
            {
                Dictionary<string, string> savedData = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { "GameModel" });

                if (savedData != null && savedData.TryGetValue("GameModel", out string gameModelData) && !string.IsNullOrEmpty(gameModelData))
                {
                    SaveGameModel saveModel = (SaveGameModel)DeserializeGameModel(gameModelData);
                    Debug.Log(gameModelData);

                    //saveModel.AssignSaveToGameModel(game); <-- Moved to CloudLoad() method in DataProvider
                    if (saveModel.lifetimeDestructionScore >= game.LifetimeDestructionScore)
                    {
                        Debug.Log("Cloud save up to date");
                        return saveModel;
                    }
                    else
                    {
                        Debug.Log("Cloud save not up to date");
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (UnityException e)
            {
                Debug.LogError("CloudLoad Error: " + e);
                return null;
            }
            catch (TimeoutException e)
            {
                Debug.LogWarning("CloudLoad Timeout Occurred: " + e);
                return null;
            }
            catch (Exception e)
            {
                Debug.LogError("CloudLoad Error: " + e);
                return null;
            }
        }

        public async void DeleteCloudSave()
        {
            await CloudSaveService.Instance.Data.ForceDeleteAsync("GameModel");
        }

        public async Task<bool> SyncCoinsToCloud()
        {
            try
            {
                await EconomyManager.SetEconomyBalance("COIN", DataProvider.GameModel.Coins);
                return true;
            }
            catch (EconomyRateLimitedException e)
            {
                Debug.LogException(e);
                return false;
            }
            catch (Exception e)
            {
                Debug.Log("Problem getting Economy currency balances:");
                Debug.LogException(e);
                return false;
            }
        }

        public async Task<bool> SyncCurrencyFromCloud()
        {
            GetBalancesResult balanceResult = null;
            try
            {
                if (this == null) return false;
                balanceResult = await EconomyManager.GetEconomyBalances();
                if (this == null) return false;
                if (balanceResult is null) return false;
                foreach (var balance in balanceResult.Balances)
                {
                    if (balance.Balance > 0 && balance.CurrencyId == "COIN")
                    {
                        DataProvider.GameModel.Coins = balance.Balance;
                    }
                    if (balance.Balance > 0 && balance.CurrencyId == "CREDIT")
                    {
                        DataProvider.GameModel.Credits = balance.Balance;
                    }
                }
                return true;
            }
            catch (EconomyRateLimitedException e)
            {
                balanceResult = await BattleCruisers.Utils.UGS.Samples.Utils.RetryEconomyFunction(EconomyManager.GetEconomyBalances, e.RetryAfter);
                if (this == null) return false;
                if (balanceResult is null) return false;
                foreach (var balance in balanceResult.Balances)
                {
                    if (balance.Balance > 0 && balance.CurrencyId == "COIN")
                    {
                        DataProvider.GameModel.Coins = balance.Balance;
                    }
                    if (balance.Balance > 0 && balance.CurrencyId == "CREDIT")
                    {
                        DataProvider.GameModel.Credits = balance.Balance;
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.Log("Problem getting Economy currency balances:");
                Debug.LogException(e);
                return false;
            }
        }

        public async Task<bool> SyncInventoryFromCloud()
        {
            GetInventoryResult inventoryResult = null;
            try
            {
                inventoryResult = await EconomyManager.GetEconomyInventories();
                if (this == null) return false;
                if (inventoryResult is null) return false;
                foreach (var inventory in inventoryResult.PlayersInventoryItems)
                {
                    if (inventory.GetItemDefinition().Name.Contains("Captain"))
                    {
                        int index = StaticPrefabKeys.CaptainItems[inventory.GetItemDefinition().Name.ToUpper()];
                        DataProvider.GameModel.AddExo(index);
                    }
                    if (inventory.GetItemDefinition().Name.Contains("Heckle"))
                    {
                        int index = StaticPrefabKeys.HeckleItems[inventory.GetItemDefinition().Name.ToUpper()];
                        DataProvider.GameModel.AddHeckle(index);
                    }
                    if (inventory.GetItemDefinition().Name.Contains("Bodykit"))
                    {
                        int index = StaticPrefabKeys.BodykitItems[inventory.GetItemDefinition().Name.ToUpper()];
                        DataProvider.GameModel.AddBodykit(index);
                    }
                }
                DataProvider.SaveGame();
                return true;
            }
            catch (EconomyRateLimitedException e)
            {
                inventoryResult = await BattleCruisers.Utils.UGS.Samples.Utils.RetryEconomyFunction(EconomyManager.GetEconomyInventories, e.RetryAfter);
                if (this == null) return false;
                if (inventoryResult is null) return false;
                foreach (var inventory in inventoryResult.PlayersInventoryItems)
                {
                    if (inventory.GetItemDefinition().Name.Contains("Captain"))
                    {
                        int index = StaticPrefabKeys.CaptainItems[inventory.GetItemDefinition().Name.ToUpper()];
                        DataProvider.GameModel.AddExo(index);
                    }
                    if (inventory.GetItemDefinition().Name.Contains("Heckle"))
                    {
                        int index = StaticPrefabKeys.HeckleItems[inventory.GetItemDefinition().Name.ToUpper()];
                        DataProvider.GameModel.AddHeckle(index);
                    }
                    if (inventory.GetItemDefinition().Name.Contains("Bodykit"))
                    {
                        int index = StaticPrefabKeys.BodykitItems[inventory.GetItemDefinition().Name.ToUpper()];
                        DataProvider.GameModel.AddBodykit(index);
                    }
                }
                DataProvider.SaveGame();
                return true;
            }
            catch (Exception e)
            {
                Debug.Log("Problem getting Economy currency balances:");
                Debug.LogException(e);
                return false;
            }
        }

        public async Task<bool> SyncCreditsToCloud()
        {
            try
            {
                await EconomyManager.SetEconomyBalance("CREDIT", DataProvider.GameModel.Credits);
                return true;
            }
            catch (EconomyRateLimitedException e)
            {
                Debug.LogException(e);
                return false;
            }
            catch (Exception e)
            {
                Debug.Log("Problem getting Economy currency balances:");
                Debug.LogException(e);
                return false;
            }
        }
    }
}
