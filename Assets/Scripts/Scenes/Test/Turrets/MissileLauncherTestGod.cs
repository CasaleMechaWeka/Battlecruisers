using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Turrets;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test
{
    public class MissileLauncherTestGod : SmartMissileTestGod
	{
		public MissileLauncher missileLauncher;

		protected override List<GameObject> GetGameObjects()
		{
			Assert.IsNotNull(missileLauncher);

			List<GameObject> gameObjects = base.GetGameObjects();
			gameObjects.Add(missileLauncher.GameObject);
			return gameObjects;
		}

        protected override async Task InitialiseMissileAsync(Helper helper, ICruiser redCruiser)
        {
			helper.InitialiseBuilding(missileLauncher, Faction.Blues, enemyCruiser: redCruiser);
			missileLauncher.StartConstruction();
		}
	}
}
