using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Aircraft
{
    public class GunshipTestGod : MonoBehaviour
    {
        public List<Vector2> gunshipPatrolPoints;

        void Start()
        {
            Helper helper = new Helper();

            // Setup gunship
            IAircraftProvider aircraftProvider = helper.CreateAircraftProvider(gunshipPatrolPoints: gunshipPatrolPoints);
            GunShipController gunship = FindObjectOfType<GunShipController>();
            helper.InitialiseUnit(gunship, Faction.Blues, aircraftProvider: aircraftProvider, parentCruiserDirection: Direction.Left);
            gunship.StartConstruction();

            // Setup target attack boats
            AttackBoatController[] ships = FindObjectsOfType<AttackBoatController>();
            foreach (AttackBoatController ship in ships)
            {
                helper.InitialiseUnit(ship, Faction.Reds);
                ship.StartConstruction();
			}
        }
    }
}
