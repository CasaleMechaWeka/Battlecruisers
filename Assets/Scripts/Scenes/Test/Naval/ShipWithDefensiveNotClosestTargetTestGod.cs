using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Turrets;

namespace BattleCruisers.Scenes.Test.Naval
{
    public class ShipWithDefensiveNotClosestTargetTestGod : ShipStopsForEnemyCruiserTestGod
	{
		protected override void Start()
		{
            base.Start();

            // Turrets
            TurretController[] turrets = FindObjectsOfType<TurretController>();
            foreach (TurretController turret in turrets)
            {
                _helper.InitialiseBuilding(turret, Faction.Reds);
                turret.StartConstruction();
			}

            // Non turret target
            IBuilding navalFactory = FindObjectOfType<NavalFactory>();
            _helper.InitialiseBuilding(navalFactory, Faction.Reds);
            navalFactory.StartConstruction();
        }
	}
}
