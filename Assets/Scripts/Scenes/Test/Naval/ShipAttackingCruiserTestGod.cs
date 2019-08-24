using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Scenes.Test.Utilities;

namespace BattleCruisers.Scenes.Test.Naval
{
    public class ShipAttackingCruiserTestGod : TestGodBase
    {
        protected override void Start()
        {
            base.Start();

            // Setup fake cruiser
            TestTarget fakeCruiser = FindObjectOfType<TestTarget>();
            fakeCruiser.Initialise(Faction.Reds);

            // Setup ship
            Helper helper = new Helper(updaterProvider: _updaterProvider);
            ShipController attackBoat = FindObjectOfType<ShipController>();
            helper.InitialiseUnit(attackBoat, Faction.Blues);
            attackBoat.StartConstruction();
        }
    }
}