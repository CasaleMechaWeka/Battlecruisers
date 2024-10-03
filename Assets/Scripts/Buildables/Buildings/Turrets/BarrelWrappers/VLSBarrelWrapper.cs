using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Utils;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public class VLSBarrelWrapper : StaticBarrelWrapper
    {
        protected override float DesiredAngleInDegrees => 90;

        protected override void InitialiseBarrelController(BarrelController barrel, IBarrelControllerArgs args)
        {
			RocketBarrelController rocketBarrel = barrel.Parse<RocketBarrelController>();
            rocketBarrel.InitialiseAsync(args);
        }
    }
}
