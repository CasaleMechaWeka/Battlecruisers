using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;

namespace BattleCruisers.Scenes.Test.Balancing.Range
{
    public class DestroyerOutOfMortarRangeTestGod : TestGodBase
    {
        protected override void Start()
        {
            base.Start();

            Helper helper = new Helper(updaterProvider: _updaterProvider);

            ICruiser blueCruiser = helper.CreateCruiser(Direction.Right, Faction.Blues);

            // Initialise mortar
            IBuilding mortar = FindObjectOfType<TurretController>();
            helper.InitialiseBuilding(mortar, Faction.Reds, parentCruiserDirection: Direction.Left);
            mortar.StartConstruction();

            // Initialise destroyers
            IUnit[] destroyers = FindObjectsOfType<ShipController>();
            foreach (IUnit destroyer in destroyers)
            {
                helper.InitialiseUnit(destroyer, Faction.Blues, parentCruiserDirection: Direction.Right, parentCruiser: blueCruiser);
                destroyer.StartConstruction();
                Helper.SetupUnitForUnitMonitor(destroyer, blueCruiser);
            }
        }
    }
}
