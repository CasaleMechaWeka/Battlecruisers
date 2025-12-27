using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Scenes;
using BattleCruisers.Data.Settings;
using Newtonsoft.Json;

namespace BattleCruisers.Tests.Data
{
    /// <summary>
    /// Tests for save version compatibility system.
    /// 
    /// Version system:
    /// - Legacy versions (0-5): Old format saves (v3, v4, v5, etc.) - migrate to current version (6.5 = 650)
    /// - Current format (640+): Modern saves using app version numbers
    ///   - 640 = 6.4.0 (in production, compatible)
    ///   - 650 = 6.5.0 (current/newest version)
    ///   - All versions >= 640 use the same format and are compatible with each other
    /// </summary>
    public class SerializerVersionTests
    {
        private Serializer _serializer;
        private string _testFilePath;
        private BinaryFormatter _binaryFormatter;

        [SetUp]
        public void SetUp()
        {
            _serializer = new Serializer();
            _binaryFormatter = new BinaryFormatter();
            
            // Create a unique temporary file path for each test
            _testFilePath = Path.Combine(Application.persistentDataPath, "test_save_" + System.Guid.NewGuid().ToString() + ".bcms");

            // Use reflection to set the private 'preferredGameModelFilePath' field to our test path
            // This prevents overwriting the actual game save
            var field = typeof(Serializer).GetField("preferredGameModelFilePath", BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null)
            {
                field.SetValue(_serializer, _testFilePath);
            }
            else
            {
                Debug.LogError("Could not find preferredGameModelFilePath field via reflection!");
            }
        }

        [TearDown]
        public void TearDown()
        {
            // Cleanup
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
        }

        /// <summary>
        /// Gets the normalized save version from Application.version (matches Serializer.GetCurrentSaveVersion logic).
        /// Extracts major.minor and converts to save format (e.g., "6.5.107" -> 650, "6.4.0" -> 640)
        /// </summary>
        private static int GetCurrentSaveVersionForTest()
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

        private void CreateSaveFile(int saveVersion)
        {
            // Create a dummy GameModel with minimal valid data
            GameModel game = new GameModel();
            
            // Setup critical fields required by validation
            // Loadout requires HullKey, Buildings list, Units list in constructor
            game.PlayerLoadout = new Loadout(
                new HullKey("TestHull"),
                new List<BuildingKey>(),
                new List<UnitKey>()
            );
            
            game.PlayerLoadout.CurrentCaptain = new CaptainExoKey("CaptainExo000");
            game.PlayerLoadout.SelectedVariants = new List<int>();
            game.PlayerLoadout.Create_buildsAnd_units(); // Ensure valid internal dicts
            
            game.SaveVersion = saveVersion; // Set the version we want to test
            
            using (FileStream file = File.Create(_testFilePath))
            {
                _binaryFormatter.Serialize(file, game);
            }
        }

        [Test]
        public void LoadGame_LegacyVersion5_MigratesToCurrent()
        {
            // Arrange: Create a v5 save (legacy format)
            CreateSaveFile(5);

            // Act: Load the game
            GameModel loadedGame = _serializer.LoadGame();

            // Assert
            // Legacy v5 should migrate to current version (6.5 = 650)
            int currentVersion = GetCurrentSaveVersionForTest();

            Assert.IsNotNull(loadedGame, "Loaded game should not be null");
            Assert.AreEqual(currentVersion, loadedGame.SaveVersion, $"Legacy v5 should migrate to current version {currentVersion}, but got {loadedGame.SaveVersion}");
        }

        [Test]
        public void LoadGame_LegacyVersion3_MigratesToCurrent()
        {
            // Arrange: Create a v3 save (legacy format)
            CreateSaveFile(3);

            // Act: Load the game
            GameModel loadedGame = _serializer.LoadGame();

            // Assert
            // Legacy v3 should migrate to current version (6.5 = 650)
            int currentVersion = GetCurrentSaveVersionForTest();

            Assert.IsNotNull(loadedGame, "Loaded game should not be null");
            Assert.AreEqual(currentVersion, loadedGame.SaveVersion, $"Legacy v3 should migrate to current version {currentVersion}, but got {loadedGame.SaveVersion}");
        }

        [Test]
        public void LoadGame_LegacyVersion4_MigratesToCurrent()
        {
            // Arrange: Create a v4 save (legacy format)
            CreateSaveFile(4);

            // Act: Load the game
            GameModel loadedGame = _serializer.LoadGame();

            // Assert
            // Legacy v4 should migrate to current version (6.5 = 650)
            int currentVersion = GetCurrentSaveVersionForTest();

            Assert.IsNotNull(loadedGame, "Loaded game should not be null");
            Assert.AreEqual(currentVersion, loadedGame.SaveVersion, $"Legacy v4 should migrate to current version {currentVersion}, but got {loadedGame.SaveVersion}");
        }

        [Test]
        public void LoadGame_CurrentVersion640_LoadsSuccessfully()
        {
            // Arrange: Create a v640 save (typical 6.4.0 save)
            CreateSaveFile(640);

            // Act
            GameModel loadedGame = _serializer.LoadGame();

            // Assert
            Assert.IsNotNull(loadedGame);
            Assert.AreEqual(640, loadedGame.SaveVersion);
        }

        [Test]
        public void LoadGame_FutureVersion650_LoadsSuccessfully()
        {
            // Arrange: Create a v650 save (typical 6.5.0 save)
            CreateSaveFile(650);

            // Act
            GameModel loadedGame = _serializer.LoadGame();

            // Assert
            Assert.IsNotNull(loadedGame);
            Assert.AreEqual(650, loadedGame.SaveVersion);
        }

        [Test]
        public void LoadGame_InvalidVersion_MigratesToCurrent()
        {
            // Arrange: Create a v50 save (unknown version between legacy 0-5 and modern 640+)
            CreateSaveFile(50);

            // Act
            GameModel loadedGame = _serializer.LoadGame();

            // Assert
            // Unknown versions should be migrated to current version (not trigger recovery)
            int currentVersion = GetCurrentSaveVersionForTest();

            Assert.IsNotNull(loadedGame);
            Assert.AreEqual(currentVersion, loadedGame.SaveVersion, "Unknown version should be migrated to current version");
            // The migrated game should preserve data (not be a fresh game)
            Assert.IsNotNull(loadedGame.PlayerLoadout, "Migrated game should preserve loadout");
        }

        [Test]
        public void CloudLoad_ExampleJson_DeserializesCorrectly()
        {
            // Arrange: Read the example JSON file
            string jsonPath = Path.Combine(Application.dataPath, "Resources_moved", "Resources", "ExampleAllUnlockedGameModel.txt");
            Assert.IsTrue(File.Exists(jsonPath), "Example JSON file should exist");
            
            string jsonContent = File.ReadAllText(jsonPath).Trim();
            // The file contains a JSON-escaped string, so we need to deserialize it as a string first
            if (jsonContent.StartsWith("\"") && jsonContent.EndsWith("\""))
            {
                // Use JsonConvert to properly unescape the JSON string
                jsonContent = JsonConvert.DeserializeObject<string>(jsonContent);
            }
            
            // Act: Deserialize
            SaveGameModel saveModel = (SaveGameModel)_serializer.DeserializeGameModel(jsonContent);
            
            // Assert: Should deserialize without errors
            Assert.IsNotNull(saveModel, "SaveGameModel should deserialize from example JSON");
            Assert.AreEqual(4, saveModel.saveVersion, "Example JSON has saveVersion 4");
            
            // Test AssignSaveToGameModel with a fresh GameModel
            GameModel testGame = new GameModel();
            testGame.PlayerLoadout = new Loadout(
                new HullKey("Trident"),
                new List<BuildingKey>(),
                new List<UnitKey>()
            );
            testGame.PlayerLoadout.CurrentCaptain = new CaptainExoKey("CaptainExo000");
            testGame.PlayerLoadout.SelectedVariants = new List<int>();
            testGame.PlayerLoadout.Create_buildsAnd_units();
            
            Assert.DoesNotThrow(() => saveModel.AssignSaveToGameModel(testGame), 
                "AssignSaveToGameModel should not throw on example JSON");
            
            // Verify some key fields were assigned
            Assert.IsNotNull(testGame.PlayerLoadout, "PlayerLoadout should be assigned");
            Assert.IsNotNull(testGame.PlayerLoadout.CurrentCaptain, "CurrentCaptain should be assigned");
            
            // Verify version was normalized (v4 should be migrated to current version)
            // GetCurrentSaveVersion extracts major.minor only (e.g., "6.5.107" -> 650, not 65107)
            int currentVersion = GetCurrentSaveVersionForTest();
            Assert.AreEqual(currentVersion, testGame.SaveVersion, 
                $"Legacy v4 save should be migrated to current version {currentVersion}");
        }

        [Test]
        public void CloudLoad_Version640_LoadsInVersion650()
        {
            // Arrange: Create a SaveGameModel with version 640 (6.4.0)
            GameModel v640Game = new GameModel();
            v640Game.SaveVersion = 640;
            v640Game.PlayerLoadout = new Loadout(
                new HullKey("Trident"),
                new List<BuildingKey>(),
                new List<UnitKey>()
            );
            v640Game.PlayerLoadout.CurrentCaptain = new CaptainExoKey("CaptainExo000");
            v640Game.PlayerLoadout.SelectedVariants = new List<int>();
            v640Game.PlayerLoadout.Create_buildsAnd_units();
            v640Game.LifetimeDestructionScore = 1000;
            v640Game.PlayerName = "TestPlayer640";
            
            SaveGameModel v640Save = new SaveGameModel(v640Game);
            string json = _serializer.SerializeGameModel(v640Save);
            
            // Act: Deserialize and assign to a 6.5 GameModel
            SaveGameModel loaded = (SaveGameModel)_serializer.DeserializeGameModel(json);
            GameModel v650Game = new GameModel();
            v650Game.PlayerLoadout = new Loadout(
                new HullKey("Trident"),
                new List<BuildingKey>(),
                new List<UnitKey>()
            );
            v650Game.PlayerLoadout.CurrentCaptain = new CaptainExoKey("CaptainExo000");
            v650Game.PlayerLoadout.SelectedVariants = new List<int>();
            v650Game.PlayerLoadout.Create_buildsAnd_units();
            
            loaded.AssignSaveToGameModel(v650Game);
            
            // Assert: Should accept v640 save in v650 (versions >= 640 are compatible)
            Assert.AreEqual(640, v650Game.SaveVersion, "Should preserve v640 save version (compatible with v650)");
            Assert.IsNotNull(v650Game.PlayerLoadout, "Loadout should be assigned");
            Assert.AreEqual("TestPlayer640", v650Game.PlayerName, "Player name should be preserved");
            Assert.AreEqual(1000, v650Game.LifetimeDestructionScore, "Lifetime score should be preserved");
        }

        [Test]
        public void CloudLoad_Version650_LoadsInVersion650()
        {
            // Arrange: Create a SaveGameModel with version 650 (6.5.0)
            GameModel v650Game = new GameModel();
            v650Game.SaveVersion = 650;
            v650Game.PlayerLoadout = new Loadout(
                new HullKey("Trident"),
                new List<BuildingKey>(),
                new List<UnitKey>()
            );
            v650Game.PlayerLoadout.CurrentCaptain = new CaptainExoKey("CaptainExo000");
            v650Game.PlayerLoadout.SelectedVariants = new List<int>();
            v650Game.PlayerLoadout.Create_buildsAnd_units();
            v650Game.LifetimeDestructionScore = 2000;
            v650Game.PlayerName = "TestPlayer650";
            
            SaveGameModel v650Save = new SaveGameModel(v650Game);
            string json = _serializer.SerializeGameModel(v650Save);
            
            // Act: Deserialize and assign
            SaveGameModel loaded = (SaveGameModel)_serializer.DeserializeGameModel(json);
            GameModel targetGame = new GameModel();
            targetGame.PlayerLoadout = new Loadout(
                new HullKey("Trident"),
                new List<BuildingKey>(),
                new List<UnitKey>()
            );
            targetGame.PlayerLoadout.CurrentCaptain = new CaptainExoKey("CaptainExo000");
            targetGame.PlayerLoadout.SelectedVariants = new List<int>();
            targetGame.PlayerLoadout.Create_buildsAnd_units();
            
            loaded.AssignSaveToGameModel(targetGame);
            
            // Assert: Should preserve v650 version
            Assert.AreEqual(650, targetGame.SaveVersion, "Should preserve v650 save version");
            Assert.AreEqual("TestPlayer650", targetGame.PlayerName, "Player name should be preserved");
            Assert.AreEqual(2000, targetGame.LifetimeDestructionScore, "Lifetime score should be preserved");
        }

        [Test]
        public void CloudLoad_LegacyVersion5_MigratesToCurrent()
        {
            // Arrange: Create a SaveGameModel with legacy version 5
            GameModel v5Game = new GameModel();
            v5Game.SaveVersion = 5;
            v5Game.PlayerLoadout = new Loadout(
                new HullKey("Trident"),
                new List<BuildingKey>(),
                new List<UnitKey>()
            );
            v5Game.PlayerLoadout.CurrentCaptain = new CaptainExoKey("CaptainExo000");
            v5Game.PlayerLoadout.SelectedVariants = new List<int>();
            v5Game.PlayerLoadout.Create_buildsAnd_units();
            v5Game.LifetimeDestructionScore = 500;
            v5Game.PlayerName = "TestPlayerV5";
            
            SaveGameModel v5Save = new SaveGameModel(v5Game);
            string json = _serializer.SerializeGameModel(v5Save);
            
            // Act: Deserialize and assign
            SaveGameModel loaded = (SaveGameModel)_serializer.DeserializeGameModel(json);
            GameModel targetGame = new GameModel();
            targetGame.PlayerLoadout = new Loadout(
                new HullKey("Trident"),
                new List<BuildingKey>(),
                new List<UnitKey>()
            );
            targetGame.PlayerLoadout.CurrentCaptain = new CaptainExoKey("CaptainExo000");
            targetGame.PlayerLoadout.SelectedVariants = new List<int>();
            targetGame.PlayerLoadout.Create_buildsAnd_units();
            
            loaded.AssignSaveToGameModel(targetGame);
            
            // Assert: Legacy v5 should be migrated to current version
            int currentVersion = GetCurrentSaveVersionForTest();
            Assert.AreEqual(currentVersion, targetGame.SaveVersion, 
                $"Legacy v5 save should be migrated to current version {currentVersion}");
            Assert.AreEqual("TestPlayerV5", targetGame.PlayerName, "Player name should be preserved");
            Assert.AreEqual(500, targetGame.LifetimeDestructionScore, "Lifetime score should be preserved");
        }

        #region Version 651 Migration Tests - Levels 32-40 to Sidequests

        /// <summary>
        /// Scenario B: Player completed levels 32-40 but NOT sidequests 0-8.
        /// Expected: Sidequests 0-8 become complete with level difficulty.
        /// </summary>
        [Test]
        public void LoadGame_Version640_LevelsCompleteOnly_MigratesSidequests()
        {
            // Arrange: v640 save with levels 32-40 complete, sidequests 0-8 NOT complete
            GameModel game = CreateBaseGameModel(640);
            
            // Add completed levels 1-40
            for (int i = 1; i <= 31; i++)
                game.AddCompletedLevel(new CompletedLevel(i, Difficulty.Normal));
            
            // Add levels 32-40 via reflection (bypasses current validation)
            AddCompletedLevelsViaReflection(game, 32, 40, Difficulty.Hard);
            
            SaveTestFile(game);
            
            // Act
            GameModel loadedGame = _serializer.LoadGame();
            
            // Assert
            Assert.IsNotNull(loadedGame);
            Assert.AreEqual(31, loadedGame.NumOfLevelsCompleted, "Should have 31 campaign levels");
            
            for (int i = 0; i <= 8; i++)
            {
                Assert.IsTrue(loadedGame.IsSideQuestCompleted(i), 
                    $"Sidequest {i} should be complete (migrated from level {i + 32})");
            }
        }

        /// <summary>
        /// Scenario C: Player completed sidequests 0-8 but NOT levels 32-40.
        /// Expected: Sidequests remain complete, no changes needed.
        /// </summary>
        [Test]
        public void LoadGame_Version640_SidequestsCompleteOnly_PreservesSidequests()
        {
            // Arrange: v640 save with sidequests 0-8 complete, levels 32-40 NOT complete
            GameModel game = CreateBaseGameModel(640);
            
            // Add completed levels 1-31 only
            for (int i = 1; i <= 31; i++)
                game.AddCompletedLevel(new CompletedLevel(i, Difficulty.Normal));
            
            // Add sidequests 0-8 as complete
            for (int i = 0; i <= 8; i++)
                game.AddCompletedSideQuest(new CompletedLevel(i, Difficulty.Easy));
            
            SaveTestFile(game);
            
            // Act
            GameModel loadedGame = _serializer.LoadGame();
            
            // Assert
            Assert.IsNotNull(loadedGame);
            Assert.AreEqual(31, loadedGame.NumOfLevelsCompleted);
            
            for (int i = 0; i <= 8; i++)
            {
                Assert.IsTrue(loadedGame.IsSideQuestCompleted(i), 
                    $"Sidequest {i} should remain complete");
            }
        }

        /// <summary>
        /// Scenario D: Player completed BOTH levels 32-40 AND sidequests 0-8.
        /// Expected: Keep higher difficulty, remove levels 32-40.
        /// </summary>
        [Test]
        public void LoadGame_Version640_BothComplete_KeepsHigherDifficulty()
        {
            // Arrange: v640 save with BOTH levels 32-40 AND sidequests 0-8 complete
            GameModel game = CreateBaseGameModel(640);
            
            for (int i = 1; i <= 31; i++)
                game.AddCompletedLevel(new CompletedLevel(i, Difficulty.Normal));
            
            // Sidequests 0-4 completed on Easy, 5-8 on Hard
            for (int i = 0; i <= 4; i++)
                game.AddCompletedSideQuest(new CompletedLevel(i, Difficulty.Easy));
            for (int i = 5; i <= 8; i++)
                game.AddCompletedSideQuest(new CompletedLevel(i, Difficulty.Hard));
            
            // Levels 32-36 completed on Hard (higher than sidequests 0-4)
            // Levels 37-40 completed on Normal (lower than sidequests 5-8)
            AddCompletedLevelsViaReflection(game, 32, 36, Difficulty.Hard);
            AddCompletedLevelsViaReflection(game, 37, 40, Difficulty.Normal);
            
            SaveTestFile(game);
            
            // Act
            GameModel loadedGame = _serializer.LoadGame();
            
            // Assert
            Assert.IsNotNull(loadedGame);
            Assert.AreEqual(31, loadedGame.NumOfLevelsCompleted, "Levels 32-40 should be removed");
            
            // Sidequests 0-4 should now be Hard (upgraded from Easy)
            for (int i = 0; i <= 4; i++)
            {
                var sq = loadedGame.CompletedSideQuests.First(s => s.LevelNum == i);
                Assert.AreEqual(Difficulty.Hard, sq.HardestDifficulty,
                    $"Sidequest {i} should be upgraded to Hard");
            }
            
            // Sidequests 5-8 should remain Hard (not downgraded)
            for (int i = 5; i <= 8; i++)
            {
                var sq = loadedGame.CompletedSideQuests.First(s => s.LevelNum == i);
                Assert.AreEqual(Difficulty.Hard, sq.HardestDifficulty,
                    $"Sidequest {i} should remain Hard");
            }
        }

        /// <summary>
        /// Scenario A: Player has neither levels 32-40 nor sidequests 0-8 complete.
        /// Expected: No migration needed, game loads normally.
        /// </summary>
        [Test]
        public void LoadGame_Version640_NeitherComplete_LoadsNormally()
        {
            // Arrange: v640 save with only levels 1-20 complete
            GameModel game = CreateBaseGameModel(640);
            
            for (int i = 1; i <= 20; i++)
                game.AddCompletedLevel(new CompletedLevel(i, Difficulty.Normal));
            
            SaveTestFile(game);
            
            // Act
            GameModel loadedGame = _serializer.LoadGame();
            
            // Assert
            Assert.IsNotNull(loadedGame);
            Assert.AreEqual(20, loadedGame.NumOfLevelsCompleted);
            Assert.AreEqual(0, loadedGame.NumOfSideQuestsCompleted);
        }

        /// <summary>
        /// Version 651+ saves should NOT be re-migrated.
        /// </summary>
        [Test]
        public void LoadGame_Version651_DoesNotReMigrate()
        {
            // Arrange: v651 save (already migrated format)
            GameModel game = CreateBaseGameModel(651);
            
            for (int i = 1; i <= 31; i++)
                game.AddCompletedLevel(new CompletedLevel(i, Difficulty.Normal));
            
            for (int i = 0; i <= 8; i++)
                game.AddCompletedSideQuest(new CompletedLevel(i, Difficulty.Hard));
            
            SaveTestFile(game);
            
            // Act
            GameModel loadedGame = _serializer.LoadGame();
            
            // Assert
            Assert.IsNotNull(loadedGame);
            Assert.AreEqual(31, loadedGame.NumOfLevelsCompleted);
            Assert.AreEqual(651, loadedGame.SaveVersion, "Version should remain 651");
        }

        /// <summary>
        /// Partial completion: Some levels 32-40 complete, some sidequests 0-8 complete (different ones).
        /// </summary>
        [Test]
        public void LoadGame_Version640_PartialCompletion_MergesCorrectly()
        {
            // Arrange: Levels 32, 34, 36 complete; Sidequests 1, 3, 5, 7 complete
            GameModel game = CreateBaseGameModel(640);
            
            for (int i = 1; i <= 31; i++)
                game.AddCompletedLevel(new CompletedLevel(i, Difficulty.Normal));
            
            // Sidequests 1, 3, 5, 7 complete (odd numbers)
            game.AddCompletedSideQuest(new CompletedLevel(1, Difficulty.Easy));
            game.AddCompletedSideQuest(new CompletedLevel(3, Difficulty.Easy));
            game.AddCompletedSideQuest(new CompletedLevel(5, Difficulty.Easy));
            game.AddCompletedSideQuest(new CompletedLevel(7, Difficulty.Easy));
            
            // Levels 32, 34, 36 complete (maps to sidequests 0, 2, 4 - even numbers)
            AddCompletedLevelsViaReflection(game, new[] { 32, 34, 36 }, Difficulty.Hard);
            
            SaveTestFile(game);
            
            // Act
            GameModel loadedGame = _serializer.LoadGame();
            
            // Assert
            Assert.IsNotNull(loadedGame);
            Assert.AreEqual(31, loadedGame.NumOfLevelsCompleted);
            
            // Sidequests 0, 2, 4 should be complete (from levels 32, 34, 36)
            Assert.IsTrue(loadedGame.IsSideQuestCompleted(0), "Sidequest 0 from level 32");
            Assert.IsTrue(loadedGame.IsSideQuestCompleted(2), "Sidequest 2 from level 34");
            Assert.IsTrue(loadedGame.IsSideQuestCompleted(4), "Sidequest 4 from level 36");
            
            // Sidequests 1, 3, 5, 7 should remain complete
            Assert.IsTrue(loadedGame.IsSideQuestCompleted(1), "Sidequest 1 preserved");
            Assert.IsTrue(loadedGame.IsSideQuestCompleted(3), "Sidequest 3 preserved");
            Assert.IsTrue(loadedGame.IsSideQuestCompleted(5), "Sidequest 5 preserved");
            Assert.IsTrue(loadedGame.IsSideQuestCompleted(7), "Sidequest 7 preserved");
            
            // Sidequests 6, 8 should NOT be complete (no source)
            Assert.IsFalse(loadedGame.IsSideQuestCompleted(6), "Sidequest 6 not complete");
            Assert.IsFalse(loadedGame.IsSideQuestCompleted(8), "Sidequest 8 not complete");
        }

        #region Test Helpers

        private GameModel CreateBaseGameModel(int saveVersion)
        {
            GameModel game = new GameModel();
            game.SaveVersion = saveVersion;
            game.PlayerLoadout = new Loadout(
                new HullKey("Trident"),
                new List<BuildingKey>(),
                new List<UnitKey>()
            );
            game.PlayerLoadout.CurrentCaptain = new CaptainExoKey("CaptainExo000");
            game.PlayerLoadout.SelectedVariants = new List<int>();
            game.PlayerLoadout.Create_buildsAnd_units();
            return game;
        }

        private void AddCompletedLevelsViaReflection(GameModel game, int startLevel, int endLevel, Difficulty difficulty)
        {
            var field = typeof(GameModel).GetField("_completedLevels", BindingFlags.NonPublic | BindingFlags.Instance);
            var completedLevels = field.GetValue(game) as List<CompletedLevel>;
            for (int i = startLevel; i <= endLevel; i++)
            {
                completedLevels.Add(new CompletedLevel(i, difficulty));
            }
        }

        private void AddCompletedLevelsViaReflection(GameModel game, int[] levels, Difficulty difficulty)
        {
            var field = typeof(GameModel).GetField("_completedLevels", BindingFlags.NonPublic | BindingFlags.Instance);
            var completedLevels = field.GetValue(game) as List<CompletedLevel>;
            foreach (int level in levels)
            {
                completedLevels.Add(new CompletedLevel(level, difficulty));
            }
        }

        private void SaveTestFile(GameModel game)
        {
            using (FileStream file = File.Create(_testFilePath))
            {
                _binaryFormatter.Serialize(file, game);
            }
        }

        #endregion

        #endregion
    }
}

