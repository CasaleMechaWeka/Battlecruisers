using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
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
    }
}

