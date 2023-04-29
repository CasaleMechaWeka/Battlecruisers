using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleLimiters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelWrappers
{
    public class PvPSamSiteBarrelWrapper : PvPDirectFireBarrelWrapper
    {
        protected override void InitialiseBarrelController(PvPBarrelController barrel, IPvPBarrelControllerArgs args)
        {
            IPvPExactMatchTargetFilter exatMatchTargetFilter = args.TargetFilter.Parse<IPvPExactMatchTargetFilter>();
            PvPSamSiteBarrelController samSiteBarrel = barrel.Parse<PvPSamSiteBarrelController>();
            samSiteBarrel.InitialiseAsync(exatMatchTargetFilter, args);
        }

        protected override IPvPTargetFilter CreateTargetFilter()
        {
            return _factoryProvider.Targets.FilterFactory.CreateExactMatchTargetFilter();
        }

        protected override IPvPAngleLimiter CreateAngleLimiter()
        {
            return _factoryProvider.Turrets.AngleLimiterFactory.CreateAntiAirLimiter();
        }
    }
}
