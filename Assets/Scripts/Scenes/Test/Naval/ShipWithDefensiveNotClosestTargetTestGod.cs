using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Scenes.Test.Utilities;

namespace BattleCruisers.Scenes.Test.Naval
{
    public class ShipWithDefensiveNotClosestTargetTestGod : TestGodBase
	{
		protected override void Start()
		{
            base.Start();

			Helper helper = new Helper(updaterProvider: _updaterProvider);

            // Turrets
            TurretController[] turrets = FindObjectsOfType<TurretController>();
            foreach (TurretController turret in turrets)
            {
                helper.InitialiseBuilding(turret, Faction.Reds);
                turret.StartConstruction();
			}

            // Fake cruiser
            IBuilding blockingCruiserImitiation = FindObjectOfType<NavalFactory>();
            helper.InitialiseBuilding(blockingCruiserImitiation, Faction.Reds);
            blockingCruiserImitiation.StartConstruction();

            // Ship
            ShipController boat = FindObjectOfType<ShipController>();
            InitialiseBoat(helper, boat, turrets);
            boat.StartConstruction();
        }

        protected virtual void InitialiseBoat(Helper helper, ShipController boat, TurretController[] turrets)
        {
			helper.InitialiseUnit(boat, Faction.Blues);
        }
	}
}
