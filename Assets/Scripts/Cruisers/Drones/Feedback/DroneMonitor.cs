using BattleCruisers.Buildables;
using BattleCruisers.Effects.Drones;
using BattleCruisers.Utils;
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Drones.Feedback
{
    /// <summary>
    /// If you have a lot of drones they make a heck of a racket.  So limit the number
    /// of drones that make a sound.
    /// </summary>
    /// FELIX  Use, test
    public class DroneMonitor : IDroneMonitor
    {
        private readonly IDroneFactory _droneFactory;
        private readonly IDictionary<Faction, int> _factionToActiveDronesCount;

        private const int MAX_ACTIVE_DRONE_AUDIO_SOURCES = 4;

        public DroneMonitor(IDroneFactory droneFactory)
        {
            Assert.IsNotNull(droneFactory);

            _droneFactory = droneFactory;
            _droneFactory.DroneCreated += _droneFactory_DroneCreated;

            _factionToActiveDronesCount = new Dictionary<Faction, int>()
            {
                {  Faction.Blues, 0 },
                { Faction.Reds, 0 }
            };
        }

        private void _droneFactory_DroneCreated(object sender, DroneCreatedEventArgs e)
        {
            e.Drone.Activated += Drone_Activated;
            e.Drone.Deactivated += Drone_Deactivated;
        }

        private void Drone_Activated(object sender, EventArgs e)
        {
            IDroneController drone = sender.Parse<IDroneController>();
            _factionToActiveDronesCount[drone.Faction]++;
        }

        private void Drone_Deactivated(object sender, EventArgs e)
        {
            IDroneController drone = sender.Parse<IDroneController>();
            _factionToActiveDronesCount[drone.Faction]--;
        }

        public bool ShouldPlaySound(Faction faction)
        {
            Assert.IsTrue(_factionToActiveDronesCount.ContainsKey(faction));

            int numOfActiveDrones = _factionToActiveDronesCount[faction];
            bool shouldPlaySound = numOfActiveDrones < MAX_ACTIVE_DRONE_AUDIO_SOURCES;
            Logging.Log(Tags.DRONE_FEEDBACK, $"Faction: {faction}  Active drone #: {numOfActiveDrones}  shouldMakeSound: {shouldPlaySound}");
            return shouldPlaySound;
        }
    }
}