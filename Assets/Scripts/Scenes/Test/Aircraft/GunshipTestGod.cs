using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Aircraft
{
    public class GunshipTestGod : TestGodBase
    {
        private AttackBoatController[] _ships;
        private GunShipController _gunship;

        public List<Vector2> gunshipPatrolPoints;

        protected override IList<GameObject> GetGameObjects()
        {
            _ships = FindObjectsOfType<AttackBoatController>();
            IList<GameObject> gameObjects = _ships.Select(ship => ship.GameObject).ToList();

            _gunship = FindObjectOfType<GunShipController>();
            gameObjects.Add(_gunship.GameObject);

            return gameObjects;
        }

        protected override void Setup(Helper helper)
        {
            ICruiser redCruiser = helper.CreateCruiser(Direction.Left, Faction.Reds);

            // Setup gunship
            IAircraftProvider aircraftProvider = helper.CreateAircraftProvider(gunshipPatrolPoints: gunshipPatrolPoints);
            helper.InitialiseUnit(_gunship, Faction.Blues, aircraftProvider: aircraftProvider, parentCruiserDirection: Direction.Left, enemyCruiser: redCruiser);
            _gunship.StartConstruction();

            // Setup target attack boats
            foreach (AttackBoatController ship in _ships)
            {
                helper.InitialiseUnit(ship, Faction.Reds);
                ship.StartConstruction();
                Helper.SetupUnitForUnitMonitor(ship, redCruiser);
			}
        }
    }
}
