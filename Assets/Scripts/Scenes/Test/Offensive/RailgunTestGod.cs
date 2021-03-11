using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Offensive
{
    public class RailgunTestGod : TestGodBase 
	{
        private IBuilding _target;
        private IBuilding _railgun;

        protected override List<GameObject> GetGameObjects()
        {
            _target = FindObjectOfType<AirFactory>();
            _railgun = FindObjectOfType<TurretController>();

            return new List<GameObject>()
            {
                _target.GameObject,
                _railgun.GameObject
            };
        }

        protected override void Setup(Helper helper)
        {
			// Setup target
			helper.InitialiseBuilding(_target, Faction.Reds);
			_target.StartConstruction();

			// Setup railgun
            ITargetFactories targetFactories = helper.CreateTargetFactories(_target.GameObject);
			helper.InitialiseBuilding(_railgun, Faction.Blues, targetFactories: targetFactories);
			_railgun.StartConstruction();
		}
	}
}
