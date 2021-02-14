using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections.Generic;
using UnityEngine;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Effects.Splash
{
    public class ShipVsShipTestGod : TestGodBase
	{
        public ShipController ship1, ship2;

        protected override List<GameObject> GetGameObjects()
        {
            BCUtils.Helper.AssertIsNotNull(ship1, ship2);

            return new List<GameObject>()
            {
                ship1.GameObject,
                ship2.GameObject
            };
        }

        protected override void Setup(Helper helper)
        {
            helper.InitialiseUnit(ship1, Faction.Blues, parentCruiserDirection: Direction.Right);
			ship1.StartConstruction();

            helper.InitialiseUnit(ship2, Faction.Reds, parentCruiserDirection: Direction.Left);
			ship2.StartConstruction();
		}
	}
}
