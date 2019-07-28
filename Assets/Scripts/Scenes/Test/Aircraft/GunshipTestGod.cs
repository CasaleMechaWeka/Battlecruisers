using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Scenes.Test.Utilities;
using NSubstitute;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Aircraft
{
    public class GunshipTestGod : TestGodBase
    {
        public List<Vector2> gunshipPatrolPoints;

        protected override void Start()
        {
            base.Start();

            Helper helper = new Helper(updaterProvider: _updaterProvider);

            ICruiser redCruiser = helper.CreateCruiser(Direction.Left, Faction.Reds);

            // Setup gunship
            IAircraftProvider aircraftProvider = helper.CreateAircraftProvider(gunshipPatrolPoints: gunshipPatrolPoints);
            GunShipController gunship = FindObjectOfType<GunShipController>();
            helper.InitialiseUnit(gunship, Faction.Blues, aircraftProvider: aircraftProvider, parentCruiserDirection: Direction.Left, enemyCruiser: redCruiser);
            gunship.StartConstruction();

            // Setup target attack boats
            AttackBoatController[] ships = FindObjectsOfType<AttackBoatController>();
            foreach (AttackBoatController ship in ships)
            {
                helper.InitialiseUnit(ship, Faction.Reds);
                ship.StartConstruction();

                // So UnitTargets knows about ships, and ManualProximityTargetProcessor works :)
                ship.CompletedBuildable += (sender, e) => redCruiser.UnitMonitor.UnitCompleted += Raise.EventWith(new UnitCompletedEventArgs(ship));
                ship.Destroyed += (sender, e) => redCruiser.UnitMonitor.UnitDestroyed += Raise.EventWith(new UnitDestroyedEventArgs(ship));
			}
        }
    }
}
