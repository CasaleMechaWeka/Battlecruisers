using BattleCruisers.Utils;
using System;
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

        private int _numOfActiveDrones = 0;
        private const int MAX_ACTIVE_DRONE_AUDIO_SOURCES = 4;

        public bool ShouldDroneMakeSound
        {
            get
            {
                bool shouldMakeSound = _numOfActiveDrones < MAX_ACTIVE_DRONE_AUDIO_SOURCES;
                Logging.Log(Tags.DRONE_FEEDBACK, $"Active drone #: {_numOfActiveDrones}  shouldMakeSound: {shouldMakeSound}");
                return shouldMakeSound;
            }
        }

        public DroneMonitor(IDroneFactory droneFactory)
        {
            Assert.IsNotNull(droneFactory);

            _droneFactory = droneFactory;
            _droneFactory.DroneCreated += _droneFactory_DroneCreated;
        }

        private void _droneFactory_DroneCreated(object sender, DroneCreatedEventArgs e)
        {
            e.Drone.Activated += Drone_Activated;
            e.Drone.Deactivated += Drone_Deactivated;
        }

        private void Drone_Activated(object sender, EventArgs e)
        {
            ++_numOfActiveDrones;
        }

        private void Drone_Deactivated(object sender, EventArgs e)
        {
            --_numOfActiveDrones;
        }
    }
}