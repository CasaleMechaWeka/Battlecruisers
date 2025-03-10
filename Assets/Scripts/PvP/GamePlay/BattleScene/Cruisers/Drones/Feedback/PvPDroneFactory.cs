using BattleCruisers.Cruisers.Drones.Feedback;
using BattleCruisers.Effects.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones.Feedback
{
    public class PvPDroneFactory : IDroneFactory
    {
        private readonly IPvPPrefabFactory _prefabFactory;

        public event EventHandler<DroneCreatedEventArgs> DroneCreated;

        public PvPDroneFactory(IPvPPrefabFactory prefabFactory)
        {
            Assert.IsNotNull(prefabFactory);
            _prefabFactory = prefabFactory;
        }

        public IDroneController CreateItem()
        {
            IDroneController newDrone = _prefabFactory.CreateDrone();
            DroneCreated?.Invoke(this, new DroneCreatedEventArgs(newDrone));
            return newDrone;
        }
    }
}