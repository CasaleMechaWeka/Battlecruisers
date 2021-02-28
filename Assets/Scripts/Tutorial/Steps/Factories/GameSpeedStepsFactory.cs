using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using System.Collections.Generic;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class GameSpeedStepsFactory : TutorialFactoryBase, ITutorialStepsFactory
    {
        private readonly IExplanationDismissableStepFactory _explanationDismissableStepFactory;
        private readonly IFeaturePermitterStepFactory _featurePermitterStepFactory;
        private readonly IPermitter _gameSpeedPermitter, _navigationPermitter;
        private readonly RightPanelComponents _rightPanelComponents;
        private readonly IUIManager _uiManager;

        public GameSpeedStepsFactory(
            ITutorialStepArgsFactory argsFactory,
            ILocTable tutorialStrings,
            IExplanationDismissableStepFactory explanationDismissableStepFactory, 
            IFeaturePermitterStepFactory featurePermitterStepFactory,
            IPermitter gameSpeedPermitter, 
            IPermitter navigationPermitter, 
            RightPanelComponents rightPanelComponents,
            IUIManager uiManager)
            : base(argsFactory, tutorialStrings)
        {
            Helper.AssertIsNotNull(
                explanationDismissableStepFactory, 
                featurePermitterStepFactory, 
                gameSpeedPermitter, 
                navigationPermitter, 
                rightPanelComponents, 
                uiManager);

            _explanationDismissableStepFactory = explanationDismissableStepFactory;
            _featurePermitterStepFactory = featurePermitterStepFactory;
            _gameSpeedPermitter = gameSpeedPermitter;
            _navigationPermitter = navigationPermitter;
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
            steps.Add(_featurePermitterStepFactory.CreateStep(_navigationPermitter, enableFeature: true));

            // Explain game speed buttons
            steps.Add(
                _explanationDismissableStepFactory.CreateStep(
                    _argsFactory.CreateTutorialStepArgs(
                        _tutorialStrings.GetString("Steps/GameSpeed/Buttons"),
                        _rightPanelComponents.SpeedComponents.SpeedButtonPanel))); ;

            // Encourage user to experiment
            steps.Add(
                _explanationDismissableStepFactory.CreateStepWithSecondaryButton(
                    _argsFactory.CreateTutorialStepArgs(
                        _tutorialStrings.GetString("Steps/GameSpeed/TryButtons"))));

            return steps;
        }
    }
}