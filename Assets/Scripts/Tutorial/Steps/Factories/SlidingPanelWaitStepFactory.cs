using BattleCruisers.Tutorial.Steps.WaitSteps;
using BattleCruisers.UI.Panels;
using BattleCruisers.Utils;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class SlidingPanelWaitStepFactory : TutorialFactoryBase
    {
        private readonly SlidingPanel _selector, _informator;

        public SlidingPanelWaitStepFactory(
            TutorialStepArgsFactory argsFactory,
            SlidingPanel selector,
            SlidingPanel informator)
            : base(argsFactory)
        {
            Helper.AssertIsNotNull(selector, informator);

            _selector = selector;
            _informator = informator;
        }

        public TutorialStep CreateInformatorShownWaitStep()
        {
            return
                new SlidingPanelWaitStep(
                    _argsFactory.CreateTutorialStepArgs(),
                    _informator,
                    desiredState: PanelState.Shown);
        }

        public TutorialStep CreateSelectorShownWaitStep()
        {
            return
                new SlidingPanelWaitStep(
                    _argsFactory.CreateTutorialStepArgs(),
                    _selector,
                    desiredState: PanelState.Shown);
        }

        public TutorialStep CreateSelectorHiddenWaitStep()
        {
            return
                new SlidingPanelWaitStep(
                    _argsFactory.CreateTutorialStepArgs(),
                    _selector,
                    desiredState: PanelState.Hidden);
        }
    }
}