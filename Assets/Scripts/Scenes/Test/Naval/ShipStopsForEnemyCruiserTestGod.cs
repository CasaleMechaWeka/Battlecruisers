using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Naval
{
    public class ShipStopsForEnemyCruiserTestGod : TestGodBase
    {
        protected override void Start()
        {
            base.Start();

            Helper helper = new Helper(updaterProvider: _updaterProvider);

            ShipController ship = FindObjectOfType<ShipController>();
            Assert.IsNotNull(ship);
            helper.InitialiseUnit(ship);
            ship.StartConstruction();
        }
    }
}