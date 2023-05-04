using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelWrappers
{
    public class PvPRocketLauncherBarrelWrapper : PvPStaticBarrelWrapper
    {
        protected override float DesiredAngleInDegrees => 60;

        protected override void InitialiseBarrelController(PvPBarrelController barrel, IPvPBarrelControllerArgs args)
        {
            PvPRocketBarrelController rocketBarrel = barrel.Parse<PvPRocketBarrelController>();
            rocketBarrel.InitialiseAsync(args);
        }
    }
}
