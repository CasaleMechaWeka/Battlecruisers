using UnityEngine;

namespace BattleCruisers.Data.Serialization
{
	public class ModelFilePathProvider : IModelFilePathProvider
	{
		private const string GAME_MODEL_FILE_NAME = "GameModel";
		private const string GAME_MODEL_FILE_EXTENSION = "bcms";

		public ModelFilePathProvider()
		{
			GameModelFilePath = Application.persistentDataPath + "/" + GAME_MODEL_FILE_NAME + "." + GAME_MODEL_FILE_EXTENSION;
		}

		public string GameModelFilePath { get; }
	}
}
