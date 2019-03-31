using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using System.Collections.Generic;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class GameSpeedStepsFactory : TutorialFactoryBase, ITutorialStepsFactory
    {
        private readonly IExplanationDismissableStepFactory _explanationDismissableStepFactory;
        private readonly IFeaturePermitterStepFactory _featurePermitterStepFactory;
        private readonly IPermitter _gameSpeedPermitter, _navigationWheelPermitter, _scrollWheelPermitter;
        private readonly RightPanelComponents _rightPanelComponents;
        private readonly IUIManager _uiManager;

        public GameSpeedStepsFactory(
            ITutorialStepArgsFactory argsFactory,
            IExplanationDismissableStepFactory explanationDismissableStepFactory, 
            IFeaturePermitterStepFactory featurePermitterStepFactory,
            IPermitter gameSpeedPermitter, 
            IPermitter navigationWheelPermitter, 
            IPermitter scrollWheelPermitter, 
            RightPanelComponents rightPanelComponents,
            IUIManager uiManager)
            : base(argsFactory)
        {
            Helper.AssertIsNotNull(
                explanationDismissableStepFactory, 
                featurePermitterStepFactory, 
                gameSpeedPermitter, 
                navigationWheelPermitter, 
                scrollWheelPermitter,
                rightPanelComponents, 
                uiManager);

            _explanationDismissableStepFactory = explanationDismissableStepFactory;
            _featurePermitterStepFactory = featurePermitterStepFactory;
            _gameSpeedPermitter = gameSpeedPermitter;
            _navigationWheelPermitter = navigationWheelPermitter;
            _scrollWheelPermitter = scrollWheelPermitter;
            _rightPanelComponents = rightPanelComponents;
            _uiManager = uiManager;
        }

        public IList<ITutorialStep> CreateSteps()
        {
            List<ITutorialStep> steps = new List<ITutorialStep>();

            // Hide informator, in case it is visible
            steps.Add(
                new HideItemDetailsStep(
                    _argsFactory.CreateTutorialStepArgs(),
                    _uiManager));

            // Enable speed buttons and navgiation wheel (before explanation so game speed
            // buttons aren't semi-transparent :P)
            steps.Add(_featurePermitterStepFactory.CreateStep(_gameSpeedPermitter, enableFeature: true));
            steps.Add(_featurePermitterStepFactory.CreateStep(_navigationWheelPermitter, enableFeature: true));
            steps.Add(_featurePermitterStepFactory.CreateStep(_scrollWheelPermitter, enableFeature: true));

            // Explain game speed buttons
            steps.Add(
                _explanationDismissableStepFactory.CreateStep(
                    _argsFactory.CreateTutorialStepArgs(
                        "These two buttons control the game speed.",
                        _rightPanelComponents.SpeedButtonPanel)));

            // Encourage user to experiment
            steps.Add(
                _explanationDismissableStepFactory.CreateStepWithSecondaryButton(
                    _argsFactory.CreateTutorialStepArgs("Play around with the speed buttons a bit.  (Click the \"Done\" when you have had enough.)")));

            return steps;
        }
    }
}