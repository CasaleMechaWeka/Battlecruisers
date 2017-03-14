using BattleCruisers.Utils;
using BattleCruisers.Buildings;
using BattleCruisers.UI.BuildMenus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers
{
	/// <summary>
	/// Initialises everything :D
	/// </summary>
	public class GameGod : MonoBehaviour 
	{
		public BuildMenuController buildMenuController;

		void Awake()
		{
			Assert.raiseExceptions = true;

			InitializeBuildMenuController();
		}

		// FELIX  Don't hardcode
		private void InitializeBuildMenuController()
		{
			IList<BuildingKey> buildingKeys = new List<BuildingKey>();

			// Factories
			buildingKeys.Add(new BuildingKey(BuildingCategory.Factory, "AirFactory"));
			buildingKeys.Add(new BuildingKey(BuildingCategory.Factory, "EngineeringBay"));

			// Tactical
			buildingKeys.Add(new BuildingKey(BuildingCategory.Tactical, "Shield"));

			// Defence
			buildingKeys.Add(new BuildingKey(BuildingCategory.Defence, "ShootyTurret"));

			// Offence
			buildingKeys.Add(new BuildingKey(BuildingCategory.Offence, "Artillery"));

			PrefabFetcher prefabFetcher = new PrefabFetcher();

			BuildingLoadout buildingLoadout = new BuildingLoadout(prefabFetcher, buildingKeys);
			buildMenuController.Initialise(buildingLoadout.BuildingGroups);
		}
	}
}
