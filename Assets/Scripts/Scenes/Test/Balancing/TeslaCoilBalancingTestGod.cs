using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Tactical.Shields;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Scenes.Test.Offensive;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Balancing
{
    public class TeslaCoilBalancingTestGod : OffensiveTurretTestGod
	{
        private TeslaCoil _tesla;

        protected override List<GameObject> GetGameObjects()
        {
            _tesla = FindObjectOfType<TeslaCoil>();

            List<GameObject> gameObjects = base.GetGameObjects();
            gameObjects.Add(_tesla.GameObject);
            return gameObjects;
        }

        protected override IBuilding GetTarget()
        {
            return FindObjectOfType<ShieldGenerator>();
        }

        protected override void Setup(Helper helper)
        {
            base.Setup(helper);

            helper.InitialiseBuilding(_tesla, Faction.Blues);
			_tesla.StartConstruction();
        }
	}
}
