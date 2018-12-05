using BattleCruisers.Tutorial.Explanation;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Highlighting.Masked;
using BattleCruisers.Tutorial.Providers;
using BattleCruisers.Tutorial.Steps;
using BattleCruisers.Tutorial.Steps.ClickSteps;
using BattleCruisers.Tutorial.Steps.FeatureModifierSteps;
using BattleCruisers.Tutorial.Steps.Providers;
using BattleCruisers.Tutorial.Steps.WaitSteps;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Threading;
using System.Collections.Generic;

namespace BattleCruisers.Tutorial
{
    public class TutorialStepsFactoryNEW : ITutorialStepsFactory
    {
        private readonly IHighlighterNEW _highlighter;
        private readonly ITextDisplayer _displayer;
        private readonly IExplanationDismissButton _explanationDismissButton;
        private readonly IVariableDelayDeferrer _deferrer;
        private readonly ITutorialArgsNEW _tutorialArgs;
        private readonly ISingleBuildableProvider _lastPlayerIncompleteBuildingStartedProvider;

        public TutorialStepsFactoryNEW(
            IHighlighterNEW highlighter,
            IExplanationPanel explanationPanel,
            IVariableDelayDeferrer deferrer,
            ITutorialArgsNEW tutorialArgs)
        {
            Helper.AssertIsNotNull(highlighter, explanationPanel, deferrer, tutorialArgs);

            _highlighter = highlighter;
            _displayer = explanationPanel.TextDisplayer;
            _explanationDismissButton = explanationPanel.DismissButton;
            _deferrer = deferrer;
            _tutorialArgs = tutorialArgs;

            _lastPlayerIncompleteBuildingStartedProvider = _tutorialArgs.TutorialProvider.CreateLastIncompleteBuildingStartedProvider(_tutorialArgs.PlayerCruiser);
        }

        public Queue<ITutorialStep> CreateTutorialSteps()
        {
            Queue<ITutorialStep> steps = new Queue<ITutorialStep>();

            // 1. Wait until initial camera movement is complete
            steps.Enqueue(CreateStep_CameraAdjustmentWaitStep());

            // 2. Player cruiser
            steps.Enqueue(CreateStep_YourCruiser());

            // 3. Navigation wheel
            steps.Enqueue(CreateSteps_NavigationWheel());

            return steps;
        }

        private ITutorialStep CreateStep_CameraAdjustmentWaitStep()
        {
            return
                new CameraAdjustmentWaitStep(
                    CreateTutorialStepArgs(),
                    _tutorialArgs.CameraComponents.CameraAdjuster);
        }

        private ITutorialStep CreateStep_YourCruiser()
        {
            ITutorialStepArgsNEW args
                = CreateTutorialStepArgs(
                    textToDisplay: "This is your awesome cruiser :D",
                    highlightableProvider: new StaticProvider<IMaskHighlightable>(_tutorialArgs.PlayerCruiser));

            return
                new ExplanationDismissableStep(
                    args,
                    _explanationDismissButton);
        }

        private IList<ITutorialStep> CreateSteps_NavigationWheel()
        {
            IList<ITutorialStep> steps = new List<ITutorialStep>();

            ITutorialStepArgsNEW navigationWheelStepArgs
                = CreateTutorialStepArgs(
                    textToDisplay: "This is the navigation wheel, which you use to navigate around the map.",
                    highlightableProvider: new StaticProvider<IMaskHighlightable>(_tutorialArgs.CameraComponents.NavigationWheel));
            steps.Add(
                new ExplanationDismissableStep(
                    navigationWheelStepArgs,
                    _explanationDismissButton));

            steps.Add(CreateStep_NavigationToggle(enableNavigation: true));

            ITutorialStepArgsNEW freeNavigationArgs
                = CreateTutorialStepArgs(
                    textToDisplay: "Drag the navigation wheel to navigate.");
            steps.Add(
                new ExplanationDismissableStep(
                    freeNavigationArgs,
                    _explanationDismissButton));

            steps.Add(CreateStep_NavigationToggle(enableNavigation: false));

            return steps;
        }

        private NavigationToggleStep CreateStep_NavigationToggle(bool enableNavigation)
        {
            return 
                new NavigationToggleStep(
                    CreateTutorialStepArgs(),
                    _tutorialArgs.TutorialProvider.IsNavigationEnabledFilter,
                    enableNavigation);
        }

        private ITutorialStepArgsNEW CreateTutorialStepArgs(
            string textToDisplay = null,
            IItemProvider<IMaskHighlightable> highlightableProvider = null)
        {
            return
                new TutorialStepArgsNEW(
                    _highlighter,
                    textToDisplay,
                    _displayer,
                    highlightableProvider ?? new StaticProvider<IMaskHighlightable>(item: null));
        }
    }
}