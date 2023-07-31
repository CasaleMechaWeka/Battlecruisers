using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using BattleCruisers.Data.Models;
using UnityEngine.Assertions;
using Newtonsoft.Json;
using Unity.Services.CloudSave;
using System.Threading.Tasks;
using UnityEngine;

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

		public object DeserializeGameModel(string gameModelJSON)
        {
			return JsonConvert.DeserializeObject<GameModel>(gameModelJSON);
        }

		public string SerializeGameModel(object gameModel)
		{
			return JsonConvert.SerializeObject(gameModel, new JsonSerializerSettings
			{
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore
			});
		}

		public async Task CloudSave(GameModel game)
		{
			try
			{
				var data = new Dictionary<string, object> { { "GameModel", SerializeGameModel(game) } };
				await CloudSaveService.Instance.Data.ForceSaveAsync(data);
			}
			catch (UnityException e)
			{
				Debug.LogException(e);
			}
		}

		public async Task<GameModel> CloudLoad()
		{
			try
			{
				Dictionary<string, string> savedData = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { "GameModel" });
				GameModel game = (GameModel)DeserializeGameModel(savedData["GameModel"]);
				Debug.Log(savedData["GameModel"]);
				return game;
			}
			catch (UnityException e)
			{
				Debug.LogException(e);
				return null;
			}
		}
	}
}
