using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Data.Serialization;
using BattleCruisers.Data.PrefabKeys;
using System.IO;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;
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
			UnityAsserts.Assert.raiseExceptions = true;

			_filePath = Application.persistentDataPath + "/sweetTestFile.bcms";

			_filePathProvider = Substitute.For<IModelFilePathProvider>();
			_filePathProvider.GameModelFilePath.Returns(_filePath);

			_serializer = new Serializer(_filePathProvider);

			_originalGameModel = new GameModel(
				numOfLevelsCompleted: 7,
				playerLoadout: CreateLoadout(),
				lastBattleResult: CreateBattleResult(),
				unlockedHulls: CreateUnlockedHulls(),
				unlockedBuildings: CreateUnlockedBuildings(),
				unlockedUnits: CreateUnlockedUnits());
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
			File.Create(_filePathProvider.GameModelFilePath);
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
	}
}
