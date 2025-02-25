using BattleCruisers.Effects.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using System;
using System.Threading.Tasks;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones.Feedback
{
    public class PvPDroneFactory : IPvPDroneFactory
    {
        private readonly IPvPPrefabFactory _prefabFactory;

        public event EventHandler<PvPDroneCreatedEventArgs> DroneCreated;

        public PvPDroneFactory(IPvPPrefabFactory prefabFactory)
        {
            Assert.IsNotNull(prefabFactory);
            _prefabFactory = prefabFactory;
        }

        public async Task<IDroneController> CreateItem()
        {
            IDroneController newDrone = await _prefabFactory.CreateDrone();
            DroneCreated?.Invoke(this, new PvPDroneCreatedEventArgs(newDrone));
            return newDrone;
        }
    }
}