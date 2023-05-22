using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Properties;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones.Feedback
{
    /// <summary>
    /// If you have a lot of drones they make a heck of a racket.  So limit the number
    /// of drones that make a sound.
    /// </summary>
    public class PvPDroneMonitor : IPvPDroneMonitor
    {
        private readonly IPvPDroneFactory _droneFactory;

        private readonly IDictionary<PvPFaction, int> _factionToActiveDroneNum;
        public IReadOnlyDictionary<PvPFaction, int> FactionToActiveDroneNum { get; }

        private readonly IPvPSettableBroadcastingProperty<bool> _playerACruiserHasActiveDrones;
        public IPvPBroadcastingProperty<bool> PlayerACruiserHasActiveDrones { get; }

        private readonly IPvPSettableBroadcastingProperty<bool> _playerBCruiserHasActiveDrones;
        public IPvPBroadcastingProperty<bool> PlayerBCruiserHasActiveDrones { get; }

        public PvPDroneMonitor(IPvPDroneFactory droneFactory)
        {
            Assert.IsNotNull(droneFactory);

            _droneFactory = droneFactory;
            _droneFactory.DroneCreated += _droneFactory_DroneCreated;

            _factionToActiveDroneNum = new Dictionary<PvPFaction, int>()
            {
                {  PvPFaction.Blues, 0 },
                { PvPFaction.Reds, 0 }
            };
            FactionToActiveDroneNum = new ReadOnlyDictionary<PvPFaction, int>(_factionToActiveDroneNum);

            _playerACruiserHasActiveDrones = new PvPSettableBroadcastingProperty<bool>(false);
            PlayerACruiserHasActiveDrones = new PvPBroadcastingProperty<bool>(_playerACruiserHasActiveDrones);

            _playerBCruiserHasActiveDrones = new PvPSettableBroadcastingProperty<bool>(false);
            PlayerBCruiserHasActiveDrones = new PvPBroadcastingProperty<bool>(_playerBCruiserHasActiveDrones);
        }

        private void _droneFactory_DroneCreated(object sender, PvPDroneCreatedEventArgs e)
        {
            e.Drone.Activated += Drone_Activated;
            e.Drone.Deactivated += Drone_Deactivated;
        }

        private void Drone_Activated(object sender, EventArgs e)
        {
            IPvPDroneController drone = sender.Parse<IPvPDroneController>();
            _factionToActiveDroneNum[drone.Faction]++;
            UpdateDroneActiveness();
        }

        private void Drone_Deactivated(object sender, EventArgs e)
        {
            IPvPDroneController drone = sender.Parse<IPvPDroneController>();
            _factionToActiveDroneNum[drone.Faction]--;
            UpdateDroneActiveness();

            Assert.IsTrue(_factionToActiveDroneNum[drone.Faction] >= 0);
        }

        private void UpdateDroneActiveness()
        {
            _playerACruiserHasActiveDrones.Value = _factionToActiveDroneNum[PvPFaction.Blues] != 0;
            _playerBCruiserHasActiveDrones.Value = _factionToActiveDroneNum[PvPFaction.Reds] != 0;
        }
    }
}