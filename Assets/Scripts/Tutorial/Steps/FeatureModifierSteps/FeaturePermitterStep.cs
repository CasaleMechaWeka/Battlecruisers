using BattleCruisers.UI.Filters;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.FeatureModifierSteps
{
    // FELIX  Test :D
    public class FeaturePermitterStep : TutorialStepNEW
    {
        private readonly BasicFilter _featurePermitter;
        private readonly bool _enableFeature;

        public FeaturePermitterStep(ITutorialStepArgsNEW args, BasicFilter featurePermitter, bool enableFeature)
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