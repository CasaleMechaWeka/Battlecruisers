using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using System.Collections.Generic;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public abstract class SwipeStepsFactoryBase : TutorialFactoryBase, ITutorialStepsFactory
    {
        protected readonly FeaturePermitterStepFactory _featurePermitterStepFactory;
        private readonly IPermitter _swipePermitter;
        private readonly ExplanationDismissableStepFactory _explanationDismissableStepFactory;
        private readonly string _messageKey;

        protected SwipeStepsFactoryBase(
            TutorialStepArgsFactory argsFactory,
            FeaturePermitterStepFactory featurePermitterStepFactory,
            IPermitter swipePermitter,
            ExplanationDismissableStepFactory explanationDismissableStepFactory,
            string messageKey)
            : base(argsFactory)
        {
            Helper.AssertIsNotNull(featurePermitterStepFactory, swipePermitter, explanationDismissableStepFactory);

            _featurePermitterStepFactory = featurePermitterStepFactory;
            _swipePermitter = swipePermitter;
            _explanationDismissableStepFactory = explanationDismissableStepFactory;
            _messageKey = messageKey;
        }

        public IList<TutorialStep> CreateSteps()
        {
            IList<TutorialStep> steps = new List<TutorialStep>();

            EnableNavigation(steps);

            steps.Add(
                _explanationDismissableStepFactory.CreateStepWithSecondaryButton(
                    _argsFactory.CreateTutorialStepArgs(
                        LocTableCache.TutorialTable.GetString(_messageKey))));

            DisableNavigation(steps);

            return steps;
        }

        protected virtual void EnableNavigation(IList<TutorialStep> steps)
        {
            steps.Add(_featurePermitterStepFactory.CreateStep(_swipePermitter, enableFeature: true));
        }

        protected virtual void DisableNavigation(IList<TutorialStep> steps)
        {
            steps.Add(_featurePermitterStepFactory.CreateStep(_swipePermitter, enableFeature: false));
        }
    }
}