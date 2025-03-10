using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using System.Collections.Generic;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class ScrollWheelStepsFactory : TutorialFactoryBase, ITutorialStepsFactory
    {
        private readonly IFeaturePermitterStepFactory _featurePermitterStepFactory;
        private readonly IPermitter _scrollWheelPermitter;
        private readonly IExplanationDismissableStepFactory _explanationDismissableStepFactory;

        public ScrollWheelStepsFactory(
            ITutorialStepArgsFactory argsFactory,
            ILocTable tutorialStrings,
            IFeaturePermitterStepFactory featurePermitterStepFactory,
            IPermitter scrollWheelPermitter,
            IExplanationDismissableStepFactory explanationDismissableStepFactory) 
            : base(argsFactory, tutorialStrings)
        {
            Helper.AssertIsNotNull(featurePermitterStepFactory, scrollWheelPermitter, explanationDismissableStepFactory);

            _featurePermitterStepFactory = featurePermitterStepFactory;
            _scrollWheelPermitter = scrollWheelPermitter;
            _explanationDismissableStepFactory = explanationDismissableStepFactory;
        }

        public IList<ITutorialStep> CreateSteps()
        {
            IList<ITutorialStep> steps = new List<ITutorialStep>();

            // Enable scroll wheel
            steps.Add(_featurePermitterStepFactory.CreateStep(_scrollWheelPermitter, enableFeature: true));

            // Explain scroll wheel and encourage user to experiment
            steps.Add(
                _explanationDismissableStepFactory.CreateStepWithSecondaryButton(
                    _argsFactory.CreateTutorialStepArgs(
                        _tutorialStrings.GetString("Steps/ScrollWheel"))));

            // Disable scroll wheel
            steps.Add(_featurePermitterStepFactory.CreateStep(_scrollWheelPermitter, enableFeature: false));

            return steps;
        }
    }
}