using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.Factories;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
    public class RailgunTestGod : MonoBehaviour 
	{
		void Start()
		{
			Helper helper = new Helper();


			// Setup target
            IBuilding target = FindObjectOfType<AirFactory>();
			helper.InitialiseBuilding(target, Faction.Reds);
			target.StartConstruction();


			// Setup railgun
            IBuilding railgun = FindObjectOfType<TurretController>();
            ITargetFactoriesProvider targetFactories = helper.CreateTargetFactories(target.GameObject);
			helper.InitialiseBuilding(railgun, Faction.Blues, targetFactories: targetFactories);
			railgun.StartConstruction();
		}
	}
}
