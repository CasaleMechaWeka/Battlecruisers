using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections.Generic;
using UnityEngine;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Offensive
{
    public class IonCannonTestGod : TestGodBase 
	{
        public TurretController ionCannon;
        public ShipController enemyShip;
        public NavalFactory enemyNavalFactory;
        public TestTarget enemyCruiser;

        protected override List<GameObject> GetGameObjects()
        {
            BCUtils.Helper.AssertIsNotNull(ionCannon, enemyShip, enemyNavalFactory, enemyCruiser);

            return new List<GameObject>()
            {
                ionCannon.GameObject,
                enemyShip.GameObject,
                enemyNavalFactory.GameObject,
                enemyCruiser.GameObject
            };
        }

        protected override void Setup(Helper helper)
        {
            // Setup targets
            helper.InitialiseUnit(enemyShip, Faction.Reds, parentCruiserDirection: Direction.Left);
            enemyShip.StartConstruction();

            helper.InitialiseBuilding(enemyNavalFactory, Faction.Reds);
            enemyNavalFactory.StartConstruction();

            enemyCruiser.Initialise(helper.CommonStrings, Faction.Reds);

			// Setup ion cannon
            ITargetFactories targetFactories = helper.CreateTargetFactories(enemyCruiser.GameObject);
			helper.InitialiseBuilding(ionCannon, Faction.Blues, targetFactories: targetFactories);
			ionCannon.StartConstruction();
		}
	}
}
