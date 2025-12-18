using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Static;
using BattleCruisers.Data.Static.Strategies.Helper;
using BattleCruisers.Scenes;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Data
{
	public class SeriliazerTests
	{
		private Serializer _serializer;

		private string gameModelFilePath;

		private GameModel _originalGameModel;
		private string _filePath;
		Dictionary<BuildingCategory, List<BuildingKey>> builds = new();
		Dictionary<UnitCategory, List<UnitKey>> unitkey = new();

		[SetUp]
		public void SetuUp()
		{
			string fileName = "sweetTestFile" + Random.value;
			_filePath = Application.persistentDataPath + "/" + fileName + ".bcms";

			gameModelFilePath = Application.persistentDataPath + "/GameModel.bcms";

			_serializer = new Serializer();

			_originalGameModel = new GameModel(
				hasSyncdShop: false,
				hasAttemptedTutorial: true,
				0,
				0,
				playerLoadout: CreateLoadout(),
				lastBattleResult: CreateBattleResult(),
				unlockedHulls: CreateUnlockedHulls(),
				unlockedBuildings: CreateUnlockedBuildings(),
				unlockedUnits: CreateUnlockedUnits(),
				saveVersion: ScreensSceneGod.VersionToInt(Application.version));
			_originalGameModel.NewHulls.AddItem(new HullKey("sup"));
			_originalGameModel.NewBuildings.AddItem(new BuildingKey(BuildingCategory.Ultra, "brah"));
			_originalGameModel.NewUnits.AddItem(new UnitKey(UnitCategory.Naval, "seeeendii"));
			_originalGameModel.SelectedLevel = 17;
			_originalGameModel.Skirmish
				= new SkirmishModel(
					Difficulty.Normal,
					true,
					StaticPrefabKeys.Hulls.Trident,
					true,
					StaticPrefabKeys.Hulls.Megalodon,
					false,
					StrategyType.Rush,
					backgroundLevelNum: 1);

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
			new HullKey("Trident"),
			new HullKey("Raptor"),
			new HullKey("Hammerhead")
		};
	}

	private List<BuildingKey> CreateUnlockedBuildings()
	{
		return new List<BuildingKey>()
		{
			new BuildingKey(BuildingCategory.Defence, "AntiShipTurret"),
			new BuildingKey(BuildingCategory.Factory, "AirFactory"),
			new BuildingKey(BuildingCategory.Tactical, "ShieldGenerator")
		};
	}

	private List<UnitKey> CreateUnlockedUnits()
	{
		return new List<UnitKey>()
		{
			new UnitKey(UnitCategory.Aircraft, "Fighter"),
			new UnitKey(UnitCategory.Naval, "Frigate")
		};
	}

		[TearDown]
		public void TearDown()
		{
			File.Delete(_filePath);
		}

		[Test]
		public void DoesSaveGameExist_False()
		{
			Assert.IsFalse(_serializer.DoesSaveGameExist());
		}

		[Test]
		public void DoesSaveGameExist_True()
		{
			FileStream savedGameFile = File.Create(gameModelFilePath);
			savedGameFile.Dispose();
			Assert.IsTrue(_serializer.DoesSaveGameExist());
		}

		[Test]
		public void SaveGame()
		{
			_serializer.SaveGame(_originalGameModel);
			Assert.IsTrue(_serializer.DoesSaveGameExist());
		}

		[Test]
		public void LoadGame()
		{
			SaveGame();
			GameModel loadedGame = _serializer.LoadGame();
			Assert.AreEqual(_originalGameModel, loadedGame);
		}

	[Test]
	public void LoadGame_NoSavedGame_ReturnsEmergencyRecovery()
	{
		// LoadGame now returns EmergencyRecovery() instead of throwing when no save exists
		GameModel result = _serializer.LoadGame();
		Assert.IsNotNull(result, "Should return a GameModel from EmergencyRecovery");
		int currentVersion = ScreensSceneGod.VersionToInt(Application.version);
		Assert.AreEqual(currentVersion, result.SaveVersion, "Should have current version");
		Assert.AreEqual("Charlie", result.PlayerName, "Should have default player name");
	}

		[Test]
		public void DeleteSavedGame()
		{
			FileStream savedGameFile = File.Create(gameModelFilePath);
			savedGameFile.Dispose();

			_serializer.DeleteSavedGame();

			Assert.IsFalse(_serializer.DoesSaveGameExist());
		}
	}
}
