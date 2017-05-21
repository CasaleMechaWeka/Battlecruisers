using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Data
{
	public interface IModelFilePathProvider
	{
		string GameModelFilePath { get; }
	}

	public class ModelFilePathProvider : IModelFilePathProvider
	{
		private const string GAME_MODEL_FILE_NAME = "GameModel";
		private const string GAME_MODEL_FILE_EXTENSION = "bcms";

		public ModelFilePathProvider()
		{
			GameModelFilePath = Application.persistentDataPath + "/" + GAME_MODEL_FILE_NAME + "." + GAME_MODEL_FILE_EXTENSION;
		}

		public string GameModelFilePath { get; private set; }
	}
}
