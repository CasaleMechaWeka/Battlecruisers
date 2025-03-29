using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Aircraft
{
    public class KamikazooTestGod : TestGodBase
    {
        private AttackBoatController[] _ships;
        private KamikazooController _kamikazoo;

        public List<Vector2> gunshipPatrolPoints;

        protected override List<GameObject> GetGameObjects()
        {
            _ships = FindObjectsOfType<AttackBoatController>();
            List<GameObject> gameObjects = _ships.Select(ship => ship.GameObject).ToList();

            _kamikazoo = FindObjectOfType<KamikazooController>();
            gameObjects.Add(_kamikazoo.GameObject);

            return gameObjects;
        }

        protected override void Setup(Helper helper)
        {
            ICruiser redCruiser = helper.CreateCruiser(Direction.Left, Faction.Reds);

            AircraftProvider aircraftProvider = helper.CreateAircraftProvider(bomberPatrolPoints: gunshipPatrolPoints);
            helper.InitialiseUnit(_kamikazoo, Faction.Blues, aircraftProvider: aircraftProvider, parentCruiserDirection: Direction.Left, enemyCruiser: redCruiser);
            _kamikazoo.StartConstruction();

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
