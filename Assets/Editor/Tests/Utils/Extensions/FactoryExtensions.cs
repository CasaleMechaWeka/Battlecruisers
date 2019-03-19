using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Construction;
using NSubstitute;

namespace BattleCruisers.Tests.Utils.Extensions
{
    public static class FactoryExtensions
    {
        // FELIX  Rename :D
        public static void StartBuildingUnit(this IFactory factory, IUnit unitToStart)
        {
            factory.StartedBuildingUnit += Raise.EventWith(new UnitStartedEventArgs(unitToStart));
        }

        public static void CompleteBuildingUnit(this IFactory factory, IUnit unitToComplete)
        {
            factory.CompletedBuildingUnit += Raise.EventWith(new UnitCompletedEventArgs(unitToComplete));
        }
    }
}