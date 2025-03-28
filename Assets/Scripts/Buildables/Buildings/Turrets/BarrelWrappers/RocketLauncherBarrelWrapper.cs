using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Utils;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public class RocketLauncherBarrelWrapper : StaticBarrelWrapper
    {
        protected override float DesiredAngleInDegrees => 60;

        protected override void InitialiseBarrelController(BarrelController barrel, BarrelControllerArgs args)
        {
            RocketBarrelController rocketBarrel = barrel.Parse<RocketBarrelController>();
            _ = rocketBarrel.InitialiseAsync(args);
        }
    }
}
