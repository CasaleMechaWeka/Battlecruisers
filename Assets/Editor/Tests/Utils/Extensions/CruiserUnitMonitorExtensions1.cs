using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Construction;
using NSubstitute;

namespace BattleCruisers.Tests.Utils.Extensions
{
    public static class CruiserUnitMonitorExtensions
    {
        public static void StartConstructingUnit(this ICruiserUnitMonitor unitMonitor, IUnit buildingToStart)
        {
            unitMonitor.UnitStarted += Raise.EventWith(new StartedUnitConstructionEventArgs(buildingToStart));
        }

        public static void CompleteConstructingBuliding(this ICruiserUnitMonitor unitMonitor, IUnit buildingToComplete)
        {
			unitMonitor.UnitCompleted += Raise.EventWith(new CompletedUnitConstructionEventArgs(buildingToComplete));
        }
    }
}