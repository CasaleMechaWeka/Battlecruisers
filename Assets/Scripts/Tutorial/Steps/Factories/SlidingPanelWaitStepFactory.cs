using BattleCruisers.Tutorial.Steps.WaitSteps;
using BattleCruisers.UI.Panels;
using BattleCruisers.Utils;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class SlidingPanelWaitStepFactory : TutorialFactoryBase, ISlidingPanelWaitStepFactory
    {
        private readonly ISlidingPanel _selector, _informator;

        public SlidingPanelWaitStepFactory(
            ITutorialStepArgsFactory argsFactory,
            ISlidingPanel selector,
            ISlidingPanel informator)
            : base(argsFactory)
        {
            Helper.AssertIsNotNull(selector, informator);

            _selector = selector;
            _informator = informator;
        }

        public ITutorialStep CreateInformatorShownWaitStep()
        {
            return
                new SlidingPanelWaitStep(
                    _argsFactory.CreateTutorialStepArgs(),
                    _informator,
                    desiredState: PanelState.Shown);
        }

        public ITutorialStep CreateSelectorShownWaitStep()
        {
            return
                new SlidingPanelWaitStep(
                    _argsFactory.CreateTutorialStepArgs(),
                    _selector,
                    desiredState: PanelState.Shown);
        }

        public ITutorialStep CreateSelectorHiddenWaitStep()
        {
            return
                new SlidingPanelWaitStep(
                    _argsFactory.CreateTutorialStepArgs(),
                    _selector,
                    desiredState: PanelState.Hidden);
        }
    }
}