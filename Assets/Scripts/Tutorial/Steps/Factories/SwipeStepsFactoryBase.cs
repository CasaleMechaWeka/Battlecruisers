using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using System.Collections.Generic;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public abstract class SwipeStepsFactoryBase : TutorialFactoryBase, ITutorialStepsFactory
    {
        protected readonly IFeaturePermitterStepFactory _featurePermitterStepFactory;
        private readonly IPermitter _swipePermitter;
        private readonly IExplanationDismissableStepFactory _explanationDismissableStepFactory;
        private readonly string _messageKey;

        protected SwipeStepsFactoryBase(
            ITutorialStepArgsFactory argsFactory,
            ILocTable tutorialStrings,
            IFeaturePermitterStepFactory featurePermitterStepFactory,
            IPermitter swipePermitter,
            IExplanationDismissableStepFactory explanationDismissableStepFactory,
            string messageKey) 
            : base(argsFactory, tutorialStrings)
        {
            Helper.AssertIsNotNull(featurePermitterStepFactory, swipePermitter, explanationDismissableStepFactory);

            _featurePermitterStepFactory = featurePermitterStepFactory;
            _swipePermitter = swipePermitter;
            _explanationDismissableStepFactory = explanationDismissableStepFactory;
            _messageKey = messageKey;
        }

        public IList<ITutorialStep> CreateSteps()
        {
            IList<ITutorialStep> steps = new List<ITutorialStep>();

            EnableNavigation(steps);

            steps.Add(
                _explanationDismissableStepFactory.CreateStepWithSecondaryButton(
                    _argsFactory.CreateTutorialStepArgs(
                        _tutorialStrings.GetString(_messageKey))));

            DisableNavigation(steps);

            return steps;
        }

        protected virtual void EnableNavigation(IList<ITutorialStep> steps)
        {
            steps.Add(_featurePermitterStepFactory.CreateStep(_swipePermitter, enableFeature: true));
        }

        protected virtual void DisableNavigation(IList<ITutorialStep> steps)
        {
            steps.Add(_featurePermitterStepFactory.CreateStep(_swipePermitter, enableFeature: false));
        }
    }
}