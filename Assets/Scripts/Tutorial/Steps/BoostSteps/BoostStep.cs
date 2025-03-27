using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.Tutorial.Steps.BoostSteps
{
    public abstract class BoostStep : TutorialStep
    {
        protected readonly GlobalBoostProviders _globalBoostProviders;
        protected readonly IBoostProvider _boostProvider;

        protected BoostStep(
            ITutorialStepArgs args,
            GlobalBoostProviders globalBoostProviders,
            IBoostProvider boostProvider)
            : base(args)
        {
            Helper.AssertIsNotNull(globalBoostProviders, boostProvider);

            _globalBoostProviders = globalBoostProviders;
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
