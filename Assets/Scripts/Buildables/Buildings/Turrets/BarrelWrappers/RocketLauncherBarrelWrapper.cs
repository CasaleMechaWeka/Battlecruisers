using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Utils;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public class RocketLauncherBarrelWrapper : StaticBarrelWrapper
    {
        protected override float DesiredAngleInDegrees => 60;

        protected override void InitialiseBarrelController(BarrelController barrel, IBarrelControllerArgs args)
        {
            Faction ownFaction = Helper.GetOppositeFaction(_enemyFaction);
			RocketBarrelController rocketBarrel = barrel.Parse<RocketBarrelController>();
            rocketBarrel.Initialise(args, ownFaction);
        }
    }
}
