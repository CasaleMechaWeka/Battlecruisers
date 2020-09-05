using BattleCruisers.Tutorial.Steps.WaitSteps;
using BattleCruisers.UI.Panels;
using BattleCruisers.Utils;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class SlidingPanelShownWaitStepFactory : TutorialFactoryBase, ISlidingPanelShownWaitStepFactory
    {
        private readonly ISlidingPanel _selector, _informator;

        public SlidingPanelShownWaitStepFactory(
            ITutorialStepArgsFactory argsFactory,
            ISlidingPanel selector,
            ISlidingPanel informator)
            : base(argsFactory)
        {
            Helper.AssertIsNotNull(selector, informator);

            _selector = selector;
            _informator = informator;
        }

        public ITutorialStep CreateSelectorShownWaitStep()
        {
            return
                new SlidingPanelShownWaitStep(
                    _argsFactory.CreateTutorialStepArgs(),
                    _selector);
        }

        public ITutorialStep CreateInformatorShownWaitStep()
        {
            return
                new SlidingPanelShownWaitStep(
                    _argsFactory.CreateTutorialStepArgs(),
                    _informator);
        }
    }
}