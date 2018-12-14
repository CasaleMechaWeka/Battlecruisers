using BattleCruisers.UI.Filters;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.FeatureModifierSteps
{
    public class FeaturePermitterStep : TutorialStep
    {
        private readonly BroadcastingFilter _featurePermitter;
        private readonly bool _enableFeature;

        public FeaturePermitterStep(ITutorialStepArgs args, BroadcastingFilter featurePermitter, bool enableFeature)
            : base(args)
        {
            Assert.IsNotNull(featurePermitter);

            _featurePermitter = featurePermitter;
            _enableFeature = enableFeature;
        }

        public override void Start(Action completionCallback)
        {
            base.Start(completionCallback);
            _featurePermitter.IsMatch = _enableFeature;
            OnCompleted();
        }
    }
}