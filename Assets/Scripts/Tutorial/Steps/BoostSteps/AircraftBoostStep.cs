using System;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Utils;

namespace BattleCruisers.Tutorial.Steps.EnemyCruiser
{
    // FELIX  Test :P
    public abstract class AircraftBoostStep : TutorialStep
    {
        protected readonly IBoostProvidersManager _boostProvidersManager;
        protected readonly IBoostProvider _boostProvider;

        protected AircraftBoostStep(
            ITutorialStepArgs args,
            IBoostProvidersManager boostProvidersManager,
            IBoostProvider boostProvider)
            : base(args)
        {
            Helper.AssertIsNotNull(boostProvidersManager, boostProvider);

            _boostProvidersManager = boostProvidersManager;
            _boostProvider = boostProvider;
        }

        public sealed override void Start(Action completionCallback)
        {
            base.Start(completionCallback);

            BoostProviderAction();
            OnCompleted();
        }

        protected abstract void BoostProviderAction();
    }
}
