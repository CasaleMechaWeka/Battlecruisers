using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Construction;
using NSubstitute;

namespace BattleCruisers.Tests.Utils.Extensions
{
    public static class CruiserUnitMonitorExtensions
    {
        public static void EmitUnitStarted(this ICruiserUnitMonitor unitMonitor, IUnit buildingToStart)
        {
            unitMonitor.UnitStarted += Raise.EventWith(new UnitStartedEventArgs(buildingToStart));
        }

        public static void EmitUnitComlpeted(this ICruiserUnitMonitor unitMonitor, IUnit buildingToComplete)
        {
			unitMonitor.UnitCompleted += Raise.EventWith(new UnitCompletedEventArgs(buildingToComplete));
        }
    }
}