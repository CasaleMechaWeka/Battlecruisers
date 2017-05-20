using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.DataModel
{
	public class Serializer
	{
		private readonly BinaryFormatter _binaryFormatter;
		private readonly string _gameModelFilePath;

		private const string GAME_MODEL_FILE_NAME = "GameModel";
		private const string GAME_MODEL_FILE_EXTENSION = "bcms";

		public Serializer()
		{
			_binaryFormatter = new BinaryFormatter();
			_gameModelFilePath = Application.persistentDataPath + "/" + GAME_MODEL_FILE_NAME + "." + GAME_MODEL_FILE_EXTENSION;
		}
		
		public bool DoesSavedGameExist()
		{
			return File.Exists(_gameModelFilePath);
		}

		public void SaveGame(GameModel game)
		{
			FileStream file = File.Create(_gameModelFilePath);
			_binaryFormatter.Serialize(file, game);
			file.Close();
		}

		public GameModel LoadGame()
		{
			Assert.IsTrue(DoesSavedGameExist());

			FileStream file = File.Open(_gameModelFilePath, FileMode.Open);
			GameModel game = (GameModel)_binaryFormatter.Deserialize(file);
			file.Close();
			return game;
		}
	}
}
