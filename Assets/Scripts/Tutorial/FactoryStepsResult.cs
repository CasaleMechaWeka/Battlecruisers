using System.Collections.Generic;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Tutorial.Providers;
using BattleCruisers.Tutorial.Steps;
using BattleCruisers.Utils;

namespace BattleCruisers.Tutorial
{
    public class FactoryStepsResult
    {
        public IList<ITutorialStep> Steps { get; private set; }
        public IProvider<IFactory> FactoryProvider { get; private set; }

        public FactoryStepsResult(IList<ITutorialStep> steps, IProvider<IFactory> factoryProvider)
        {
            Helper.AssertIsNotNull(steps, factoryProvider);

            Steps = steps;
            FactoryProvider = factoryProvider;
        }
    }
}
