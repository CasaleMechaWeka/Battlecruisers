using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Scenes.Test.Utilities;

namespace BattleCruisers.Scenes.Test.Projectiles
{
    public class ProjectileVisibilityTestGod : NavigationTestGod
    {
        protected override void Start()
        {
            base.Start();

            Helper helper = new Helper(updaterProvider: _updaterProvider);

            Artillery artillery = FindObjectOfType<Artillery>();
            helper.InitialiseBuilding(artillery, Faction.Blues);
            artillery.StartConstruction();

            AirFactory airFactory = FindObjectOfType<AirFactory>();
            helper.InitialiseBuilding(airFactory, Faction.Reds);
            airFactory.StartConstruction();
        }
    }
}