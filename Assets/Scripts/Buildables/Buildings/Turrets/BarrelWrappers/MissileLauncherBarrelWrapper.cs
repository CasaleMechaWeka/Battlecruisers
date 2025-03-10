using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Utils;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public class MissileLauncherBarrelWrapper : StaticBarrelWrapper
    {
        protected override float DesiredAngleInDegrees => 80;

        protected override void InitialiseBarrelController(BarrelController barrel, IBarrelControllerArgs args)
        {
			MissileBarrelController missileBarrel = barrel.Parse<MissileBarrelController>();
            missileBarrel.InitialiseAsync(args);
        }
    }
}
