using System;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Utils;

namespace BattleCruisers.Tutorial.Steps.EnemyCruiser
{
    public abstract class BoostStep : TutorialStepNEW
    {
        protected readonly IGlobalBoostProviders _globalBoostProviders;
        protected readonly IBoostProvider _boostProvider;

        protected BoostStep(
            ITutorialStepArgsNEW args,
            IGlobalBoostProviders globalBoostProviders,
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
