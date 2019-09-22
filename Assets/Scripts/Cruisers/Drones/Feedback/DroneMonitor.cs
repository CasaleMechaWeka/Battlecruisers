using BattleCruisers.Buildables;
using BattleCruisers.Effects.Drones;
using BattleCruisers.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Drones.Feedback
{
    /// <summary>
    /// If you have a lot of drones they make a heck of a racket.  So limit the number
    /// of drones that make a sound.
    /// </summary>
    public class DroneMonitor : IDroneMonitor
    {
        private readonly IDroneFactory _droneFactory;

        private readonly IDictionary<Faction, int> _factionToActiveDroneNum;
        public IReadOnlyDictionary<Faction, int> FactionToActiveDroneNum { get; }

        public DroneMonitor(IDroneFactory droneFactory)
        {
            Assert.IsNotNull(droneFactory);

            _droneFactory = droneFactory;
            _droneFactory.DroneCreated += _droneFactory_DroneCreated;

            _factionToActiveDroneNum = new Dictionary<Faction, int>()
            {
                {  Faction.Blues, 0 },
                { Faction.Reds, 0 }
            };
            FactionToActiveDroneNum = new ReadOnlyDictionary<Faction, int>(_factionToActiveDroneNum);
        }

        private void _droneFactory_DroneCreated(object sender, DroneCreatedEventArgs e)
        {
            e.Drone.Activated += Drone_Activated;
            e.Drone.Deactivated += Drone_Deactivated;
        }

        private void Drone_Activated(object sender, EventArgs e)
        {
            IDroneController drone = sender.Parse<IDroneController>();
            _factionToActiveDroneNum[drone.Faction]++;
        }

        private void Drone_Deactivated(object sender, EventArgs e)
        {
            IDroneController drone = sender.Parse<IDroneController>();
            _factionToActiveDroneNum[drone.Faction]--;

            Assert.IsTrue(_factionToActiveDroneNum[drone.Faction] >= 0);
        }
    }
}