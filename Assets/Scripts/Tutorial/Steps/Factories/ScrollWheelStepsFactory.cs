using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using System.Collections.Generic;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class ScrollWheelStepsFactory : TutorialFactoryBase, ITutorialStepsFactory
    {
        private readonly FeaturePermitterStepFactory _featurePermitterStepFactory;
        private readonly IPermitter _scrollWheelPermitter;
        private readonly ExplanationDismissableStepFactory _explanationDismissableStepFactory;

        public ScrollWheelStepsFactory(
            TutorialStepArgsFactory argsFactory,
            FeaturePermitterStepFactory featurePermitterStepFactory,
            IPermitter scrollWheelPermitter,
            ExplanationDismissableStepFactory explanationDismissableStepFactory)
            : base(argsFactory)
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
                        LocTableCache.TutorialTable.GetString("Steps/ScrollWheel"))));

            // Disable scroll wheel
            steps.Add(_featurePermitterStepFactory.CreateStep(_scrollWheelPermitter, enableFeature: false));

            return steps;
        }
    }
}