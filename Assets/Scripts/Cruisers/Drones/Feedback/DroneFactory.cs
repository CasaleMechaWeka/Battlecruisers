using BattleCruisers.Effects;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.Fetchers;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Drones.Feedback
{
    public class DroneFactory : IPoolableFactory<IDroneController, Vector2>
    {
        private readonly IPrefabFactory _prefabFactory;

        public DroneFactory(IPrefabFactory prefabFactory)
        {
            Assert.IsNotNull(prefabFactory);
            _prefabFactory = prefabFactory;
        }

        public IDroneController CreateItem()
        {
            return _prefabFactory.CreateDrone();
        }
    }
}