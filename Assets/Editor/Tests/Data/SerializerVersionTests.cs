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

namespace BattleCruisers.Tests.Data
{
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
            // Arrange: Create a v5 save
            CreateSaveFile(5);

            // Act: Load the game
            GameModel loadedGame = _serializer.LoadGame();

            // Assert
            // It should have migrated to the current version (e.g. 640 or whatever Application.version is)
            int currentVersion = ScreensSceneGod.VersionToInt(Application.version);

            Assert.IsNotNull(loadedGame, "Loaded game should not be null");
            Assert.AreEqual(currentVersion, loadedGame.SaveVersion, $"Expected version {currentVersion} (migrated), but got {loadedGame.SaveVersion}");
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
        public void LoadGame_InvalidVersion_TriggersRecovery()
        {
            // Arrange: Create a v50 save (too old, not legacy v0-5, not current v640+)
            CreateSaveFile(50);

            // Act
            GameModel loadedGame = _serializer.LoadGame();

            // Assert
            // Should trigger emergency recovery which returns a FRESH game model (SaveVersion = current)
            int currentVersion = ScreensSceneGod.VersionToInt(Application.version);

            Assert.IsNotNull(loadedGame);
            Assert.AreEqual(currentVersion, loadedGame.SaveVersion, "Should have reset to current version");
            // We can verify it's a fresh game by checking default values if needed, 
            // e.g. PlayerName should be default "Charlie"
            Assert.AreEqual("Charlie", loadedGame.PlayerName);
        }
    }
}

