using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Scenes.Test.Utilities;

namespace BattleCruisers.Scenes.Test.Turrets.AntiShip
{
    public class AntiShipTurretTestsGod : TestGodBase
	{
        protected override void Start()
        {
            base.Start();

            Helper helper = new Helper(updaterProvider: _updaterProvider);

            AttackBoatController boat = FindObjectOfType<AttackBoatController>();
            helper.InitialiseUnit(boat, Faction.Blues);
			boat.StartConstruction();

            TurretController turret = FindObjectOfType<TurretController>();
            helper.InitialiseBuilding(turret, Faction.Reds);
			turret.StartConstruction();
		}
	}
}
