using System;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Tutorial.Providers;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.EnemyCruiser
{
    // FELIX  Write tests
    public class StopUnitConstructionStep : TutorialStep
    {
        private readonly IProvider<IFactory> _factoryProvider;

        public StopUnitConstructionStep(ITutorialStepArgs args, IProvider<IFactory> factoryProvider)
            : base(args)
        {
            Assert.IsNotNull(factoryProvider);
            _factoryProvider = factoryProvider;
        }

        public override void Start(Action completionCallback)
        {
            base.Start(completionCallback);

            _factoryProvider.FindItem().UnitWrapper = null;

            OnCompleted();
        }
    }
}
