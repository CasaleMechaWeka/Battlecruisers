using BattleCruisers.Cruisers.Drones.Feedback;
using BattleCruisers.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BattleCruisers.Utils.Properties;
using UnityEngine.Assertions;
using BattleCruisers.Buildables;
using BattleCruisers.Effects.Drones;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones.Feedback
{
    /// <summary>
    /// If you have a lot of drones they make a heck of a racket.  So limit the number
    /// of drones that make a sound.
    /// </summary>
    public class PvPDroneMonitor : IPvPDroneMonitor
    {
        private readonly IDroneFactory _droneFactory;

        private readonly IDictionary<Faction, int> _factionToActiveDroneNum;
        public IReadOnlyDictionary<Faction, int> FactionToActiveDroneNum { get; }

        private readonly ISettableBroadcastingProperty<bool> _playerACruiserHasActiveDrones;
        public IBroadcastingProperty<bool> PlayerACruiserHasActiveDrones { get; }

        private readonly ISettableBroadcastingProperty<bool> _playerBCruiserHasActiveDrones;
        public IBroadcastingProperty<bool> PlayerBCruiserHasActiveDrones { get; }

        public PvPDroneMonitor(IDroneFactory droneFactory)
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

            _playerACruiserHasActiveDrones = new SettableBroadcastingProperty<bool>(false);
            PlayerACruiserHasActiveDrones = new BroadcastingProperty<bool>(_playerACruiserHasActiveDrones);

            _playerBCruiserHasActiveDrones = new SettableBroadcastingProperty<bool>(false);
            PlayerBCruiserHasActiveDrones = new BroadcastingProperty<bool>(_playerBCruiserHasActiveDrones);
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

            // Assert.IsTrue(_factionToActiveDroneNum[drone.Faction] >= 0);
            if (_factionToActiveDroneNum[drone.Faction] < 0)
                _factionToActiveDroneNum[drone.Faction] = 0;
        }

        private void UpdateDroneActiveness()
        {
            _playerACruiserHasActiveDrones.Value = _factionToActiveDroneNum[Faction.Blues] != 0;
            _playerBCruiserHasActiveDrones.Value = _factionToActiveDroneNum[Faction.Reds] != 0;
        }
    }
}