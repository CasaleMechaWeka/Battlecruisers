using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using BattleCruisers.DataModel;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using System.IO;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.DataModel
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

			_originalGameModel = new GameModel() 
			{
				numOfLevelsUnlocked = 7
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
