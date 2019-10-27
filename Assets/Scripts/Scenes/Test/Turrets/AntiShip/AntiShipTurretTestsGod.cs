using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Turrets.AntiShip
{
    public class AntiShipTurretTestsGod : TestGodBase
	{
        private AttackBoatController _boat;
        private TurretController _turret;

        protected override List<GameObject> GetGameObjects()
        {
            _boat = FindObjectOfType<AttackBoatController>();
            _turret = FindObjectOfType<TurretController>();

            return new List<GameObject>()
            {
                _boat.GameObject,
                _turret.GameObject
            };
        }

        protected override void Setup(Helper helper)
        {
            helper.InitialiseUnit(_boat, Faction.Blues);
			_boat.StartConstruction();

            helper.InitialiseBuilding(_turret, Faction.Reds);
			_turret.StartConstruction();
		}
	}
}
