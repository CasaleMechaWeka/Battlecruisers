using BattleCruisers.Tutorial.Explanation;
using BattleCruisers.Tutorial.Steps.Providers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public abstract class TutorialFactoryBase
    {
        protected readonly ITutorialStepArgsFactory _argsFactory;
        protected readonly IExplanationDismissButton _explanationDismissButton;
        // FELIX  Make each factory only require what it needs, instead of every factory having access to everything??
        protected readonly IVariableDelayDeferrer _deferrer;
        protected readonly ITutorialArgs _tutorialArgs;
        protected readonly ISingleBuildableProvider _lastPlayerIncompleteBuildingStartedProvider;

        protected TutorialFactoryBase(
            ITutorialStepArgsFactory argsFactory,
            IExplanationDismissButton explanationDismissButton,
            IVariableDelayDeferrer deferrer,
            ITutorialArgs tutorialArgs)
        {
            Helper.AssertIsNotNull(argsFactory, explanationDismissButton, deferrer, tutorialArgs);

            _argsFactory = argsFactory;
            _explanationDismissButton = explanationDismissButton;
            _deferrer = deferrer;
            _tutorialArgs = tutorialArgs;

            _lastPlayerIncompleteBuildingStartedProvider = _tutorialArgs.TutorialProvider.CreateLastIncompleteBuildingStartedProvider(_tutorialArgs.PlayerCruiser);
        }
   }
}