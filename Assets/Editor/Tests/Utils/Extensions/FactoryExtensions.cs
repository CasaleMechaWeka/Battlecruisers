using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Construction;
using NSubstitute;

namespace BattleCruisers.Tests.Utils.Extensions
{
    public static class FactoryExtensions
    {
        public static void EmitUnitStarted(this IFactory factory, IUnit unitToStart)
        {
            factory.UnitStarted += Raise.EventWith(new UnitStartedEventArgs(unitToStart));
        }

        public static void EmitUnitCompleted(this IFactory factory, IUnit unitToComplete)
        {
            factory.UnitCompleted += Raise.EventWith(new UnitCompletedEventArgs(unitToComplete));
        }
    }
}