using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using BattleCruisers.Data.Models;
using UnityEngine.Assertions;

namespace BattleCruisers.Data.Serialization
{
	public class Serializer : ISerializer
	{
		private readonly IModelFilePathProvider _modelFilePathProvider;
		private readonly BinaryFormatter _binaryFormatter;

		public Serializer(IModelFilePathProvider modelFilePathProvider)
		{
			_modelFilePathProvider = modelFilePathProvider;
			_binaryFormatter = new BinaryFormatter();
		}
		
		public bool DoesSavedGameExist()
		{
			return File.Exists(_modelFilePathProvider.GameModelFilePath);
		}

		public void SaveGame(GameModel game)
		{
			FileStream file = File.Create(_modelFilePathProvider.GameModelFilePath);
			_binaryFormatter.Serialize(file, game);
			file.Close();
		}

		public GameModel LoadGame()
		{
			Assert.IsTrue(DoesSavedGameExist());

			FileStream file = File.Open(_modelFilePathProvider.GameModelFilePath, FileMode.Open);
	
			GameModel game = (GameModel)_binaryFormatter.Deserialize(file);
			file.Close();
			return game;
		}

        public void DeleteSavedGame()
        {
            if (DoesSavedGameExist())
            {
				File.Delete(_modelFilePathProvider.GameModelFilePath);
            }
        }
    }
}
