using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Serialization;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Static;
using BattleCruisers.Data.Static.Strategies.Helper;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Data
{
    public class SeriliazerTests 
	{
		private ISerializer _serializer;

		private IModelFilePathProvider _filePathProvider;
		private GameModel _originalGameModel;
		private string _filePath;

		[SetUp]
		public void SetuUp()
		{
            string fileName = "sweetTestFile" + Random.value;
			_filePath = Application.persistentDataPath + "/" + fileName + ".bcms";

			_filePathProvider = Substitute.For<IModelFilePathProvider>();
			_filePathProvider.GameModelFilePath.Returns(_filePath);

			_serializer = new Serializer(_filePathProvider);

			_originalGameModel = new GameModel(
                hasAttemptedTutorial: true,
				playerLoadout: CreateLoadout(),
				lastBattleResult: CreateBattleResult(),
				unlockedHulls: CreateUnlockedHulls(),
				unlockedBuildings: CreateUnlockedBuildings(),
				unlockedUnits: CreateUnlockedUnits());
            _originalGameModel.NewHulls.AddItem(new HullKey("sup"));
            _originalGameModel.NewBuildings.AddItem(new BuildingKey(BuildingCategory.Ultra, "brah"));
            _originalGameModel.NewUnits.AddItem(new UnitKey(UnitCategory.Naval, "seeeendii"));
			_originalGameModel.SelectedLevel = 17;
			_originalGameModel.Skirmish = new SkirmishModel(Difficulty.Normal, StaticPrefabKeys.Hulls.Megalodon, StrategyType.Rush);

            _originalGameModel.AddCompletedLevel(new CompletedLevel(levelNum: 1, hardestDifficulty: Difficulty.Easy));
            _originalGameModel.AddCompletedLevel(new CompletedLevel(levelNum: 2, hardestDifficulty: Difficulty.Harder));

			_originalGameModel.Settings.Version = SettingsModel.ModelVersion.WithMusicVolume;
			_originalGameModel.Settings.AIDifficulty = Difficulty.Harder;
			_originalGameModel.Settings.MusicVolume = 0.25f;
			_originalGameModel.Settings.EffectVolume = 0.75f;
			_originalGameModel.Settings.ScrollSpeedLevel = 7;
			_originalGameModel.Settings.ZoomSpeedLevel = 3;
			_originalGameModel.Settings.ShowInGameHints = false;

			_originalGameModel.Hotkeys.EnemyCruiser = KeyCode.A;
			_originalGameModel.Hotkeys.Overview = KeyCode.B;
			_originalGameModel.Hotkeys.PlayerCruiser = KeyCode.C;
		}

		private Loadout CreateLoadout()
		{
			return new Loadout(
				hull: new HullKey("Schule"),
				buildings: CreateUnlockedBuildings(),
				units: CreateUnlockedUnits());
		}

		private BattleResult CreateBattleResult()
		{
			return new BattleResult(
				levelNum: 2,
				wasVictory: true);
		}

		private List<HullKey> CreateUnlockedHulls()
		{
			return new List<HullKey>() 
			{
				new HullKey("Bergsteiger"),
				new HullKey("Abstieg")
			};
		}

		private List<BuildingKey> CreateUnlockedBuildings()
		{
			return new List<BuildingKey>() 
			{
				new BuildingKey(BuildingCategory.Defence, "Ritter"),
				new BuildingKey(BuildingCategory.Factory, "Medizin"),
				new BuildingKey(BuildingCategory.Tactical, "Prinzessin")
			};
		}

		private List<UnitKey> CreateUnlockedUnits()
		{
			return new List<UnitKey>() 
			{
				new UnitKey(UnitCategory.Aircraft, "Messerschmitt"),
				new UnitKey(UnitCategory.Naval, "Herzog")
			};
		}

		[TearDown]
		public void TearDown()
		{
			File.Delete(_filePath);
		}

		[Test]
		public void DoesSavedGameExist_False()
		{
			Assert.IsFalse(_serializer.DoesSavedGameExist());
		}

		[Test]
		public void DoesSavedGameExist_True()
		{
			FileStream savedGameFile = File.Create(_filePathProvider.GameModelFilePath);
            savedGameFile.Dispose();
			Assert.IsTrue(_serializer.DoesSavedGameExist());
		}

		[Test]
		public void SaveGame()
		{
			_serializer.SaveGame(_originalGameModel);
			Assert.IsTrue(_serializer.DoesSavedGameExist());
		}

		[Test]
		public void LoadGame()
		{
			SaveGame();
			GameModel loadedGame = _serializer.LoadGame();
			Assert.AreEqual(_originalGameModel, loadedGame);
		}

		[Test]
		public void LoadGame_NoSavedGameThrows()
		{
			Assert.Throws<UnityAsserts.AssertionException>(() => _serializer.LoadGame());
		}

        [Test]
        public void DeleteSavedGame()
        {
            FileStream savedGameFile = File.Create(_filePathProvider.GameModelFilePath);
            savedGameFile.Dispose();

            _serializer.DeleteSavedGame();

            Assert.IsFalse(_serializer.DoesSavedGameExist());
        }
    }
}
