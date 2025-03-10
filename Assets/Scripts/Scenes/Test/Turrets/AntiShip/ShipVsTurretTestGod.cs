using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections.Generic;
using UnityEngine;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Turrets.AntiShip
{
    public class ShipVsTurretTestGod : TestGodBase
	{
        public ShipController ship;
        public TurretController turret;

        protected override List<GameObject> GetGameObjects()
        {
            BCUtils.Helper.AssertIsNotNull(ship, turret);

            return new List<GameObject>()
            {
                ship.GameObject,
                turret.GameObject
            };
        }

        protected override void Setup(Helper helper)
        {
            helper.InitialiseUnit(ship, Faction.Blues);
			ship.StartConstruction();

            helper.InitialiseBuilding(turret, Faction.Reds);
			turret.StartConstruction();
		}
	}
}
