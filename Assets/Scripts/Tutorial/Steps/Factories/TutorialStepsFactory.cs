using BattleCruisers.Tutorial.Explanation;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Steps.Providers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Threading;
using System.Collections.Generic;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    // FELIX  Split up monster class?  :P
    public abstract class TutorialStepsFactory : ITutorialStepsFactory
    {
        private readonly IHighlighter _highlighter;
        private readonly ITextDisplayer _displayer;
        protected readonly IExplanationDismissButton _explanationDismissButton;
        // FELIX  Make each factory only require what it needs, instead of every factory having access to everything??
        protected readonly IVariableDelayDeferrer _deferrer;
        protected readonly ITutorialArgs _tutorialArgs;
        protected readonly ISingleBuildableProvider _lastPlayerIncompleteBuildingStartedProvider;

        protected TutorialStepsFactory(
            IHighlighter highlighter,
            IExplanationPanel explanationPanel,
            IVariableDelayDeferrer deferrer,
            ITutorialArgs tutorialArgs)
        {
            Helper.AssertIsNotNull(highlighter, explanationPanel, deferrer, tutorialArgs);

            _highlighter = highlighter;
            _displayer = explanationPanel.TextDisplayer;
            _explanationDismissButton = explanationPanel.DismissButton;
            _deferrer = deferrer;
            _tutorialArgs = tutorialArgs;

            _lastPlayerIncompleteBuildingStartedProvider = _tutorialArgs.TutorialProvider.CreateLastIncompleteBuildingStartedProvider(_tutorialArgs.PlayerCruiser);
        }

        public abstract IList<ITutorialStep> CreateTutorialSteps();
   }
}