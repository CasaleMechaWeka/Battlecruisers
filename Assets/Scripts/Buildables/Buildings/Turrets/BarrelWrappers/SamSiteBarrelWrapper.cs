using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Targets.Factories;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public class SamSiteBarrelWrapper : DirectFireBarrelWrapper
    {
        protected override void InitialiseBarrelController(BarrelController barrel, IBarrelControllerArgs args)
        {
            IExactMatchTargetFilter exatMatchTargetFilter = args.TargetFilter.Parse<IExactMatchTargetFilter>();
            SamSiteBarrelController samSiteBarrel = barrel.Parse<SamSiteBarrelController>();
            _ = samSiteBarrel.InitialiseAsync(exatMatchTargetFilter, args);
        }

        protected override ITargetFilter CreateTargetFilter()
        {
            return TargetFilterFactory.CreateExactMatchTargetFilter();
        }

        protected override AngleLimiter CreateAngleLimiter()
        {
            return new AngleLimiter(30, 150);
        }
    }
}
