using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Tutorial.Providers;
using BattleCruisers.Utils;
using System.Collections.Generic;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class FactoryStepsResult
    {
        public IList<ITutorialStep> Steps { get; }
        public IItemProvider<IFactory> FactoryProvider { get; }

        public FactoryStepsResult(IList<ITutorialStep> steps, IItemProvider<IFactory> factoryProvider)
        {
            Helper.AssertIsNotNull(steps, factoryProvider);

            Steps = steps;
            FactoryProvider = factoryProvider;
        }
    }
}
