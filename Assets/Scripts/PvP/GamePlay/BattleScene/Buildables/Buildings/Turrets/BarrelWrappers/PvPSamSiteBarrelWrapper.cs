using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories;
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
            return PvPTargetFactoriesProvider.FilterFactory.CreateExactMatchTargetFilter();
        }

        protected override AngleLimiter CreateAngleLimiter()
        {
            return new AngleLimiter(30, 150);
        }
    }
}
