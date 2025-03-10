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
    public class BroadswordTestGod : TestGodBase
    {
        private AttackBoatController[] _ships;
        private BroadswordController _broadsword;

        public List<Vector2> gunshipPatrolPoints;

        protected override List<GameObject> GetGameObjects()
        {
            _ships = FindObjectsOfType<AttackBoatController>();
            List<GameObject> gameObjects = _ships.Select(ship => ship.GameObject).ToList();

            _broadsword = FindObjectOfType<BroadswordController>();
            gameObjects.Add(_broadsword.GameObject);

            return gameObjects;
        }

        protected override void Setup(Helper helper)
        {
            ICruiser redCruiser = helper.CreateCruiser(Direction.Left, Faction.Reds);

            // Setup Broadsword
            IAircraftProvider aircraftProvider = helper.CreateAircraftProvider(gunshipPatrolPoints: gunshipPatrolPoints);
            helper.InitialiseUnit(_broadsword, Faction.Blues, aircraftProvider: aircraftProvider, parentCruiserDirection: Direction.Left, enemyCruiser: redCruiser);
            _broadsword.StartConstruction();

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
