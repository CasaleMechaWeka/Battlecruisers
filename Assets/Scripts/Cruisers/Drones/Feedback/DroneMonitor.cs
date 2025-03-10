using BattleCruisers.Buildables;
using BattleCruisers.Effects.Drones;
using BattleCruisers.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BattleCruisers.Utils.Properties;
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

        private readonly ISettableBroadcastingProperty<bool> _leftCruiserHasActiveDrones;
        public IBroadcastingProperty<bool> LeftCruiserHasActiveDrones { get; }

        private readonly ISettableBroadcastingProperty<bool> _rightCruiserHasActiveDrones;
        public IBroadcastingProperty<bool> RightCruiserHasActiveDrones { get; }

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

            _leftCruiserHasActiveDrones = new SettableBroadcastingProperty<bool>(false);
            LeftCruiserHasActiveDrones = new BroadcastingProperty<bool>(_leftCruiserHasActiveDrones);

            _rightCruiserHasActiveDrones = new SettableBroadcastingProperty<bool>(false);
            RightCruiserHasActiveDrones = new BroadcastingProperty<bool>(_rightCruiserHasActiveDrones);
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
            UpdateDroneActiveness();
        }

        private void Drone_Deactivated(object sender, EventArgs e)
        {
            IDroneController drone = sender.Parse<IDroneController>();
            _factionToActiveDroneNum[drone.Faction]--;
            UpdateDroneActiveness();

            Assert.IsTrue(_factionToActiveDroneNum[drone.Faction] >= 0);
            if (_factionToActiveDroneNum[drone.Faction] < 0)
                _factionToActiveDroneNum[drone.Faction] = 0;
        }

        private void UpdateDroneActiveness()
        {
            _leftCruiserHasActiveDrones.Value = _factionToActiveDroneNum[Faction.Blues] != 0;
            _rightCruiserHasActiveDrones.Value = _factionToActiveDroneNum[Faction.Reds] != 0;
        }
    }
}