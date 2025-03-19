using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelWrappers
{
    public class PvPSamSiteBarrelWrapper : PvPDirectFireBarrelWrapper
    {
        protected override void InitialiseBarrelController(PvPBarrelController barrel, IPvPBarrelControllerArgs args)
        {
            IExactMatchTargetFilter exatMatchTargetFilter = args.TargetFilter.Parse<IExactMatchTargetFilter>();
            PvPSamSiteBarrelController samSiteBarrel = barrel.Parse<PvPSamSiteBarrelController>();
            _ = samSiteBarrel.InitialiseAsync(exatMatchTargetFilter, args);
        }

        protected override ITargetFilter CreateTargetFilter()
        {
            return _factoryProvider.Targets.FilterFactory.CreateExactMatchTargetFilter();
        }

        protected override IAngleLimiter CreateAngleLimiter()
        {
            return new AngleLimiter(30, 150);
        }
    }
}
