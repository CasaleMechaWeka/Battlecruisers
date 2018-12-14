using BattleCruisers.Utils;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public abstract class TutorialFactoryBase
    {
        protected readonly ITutorialStepArgsFactory _argsFactory;
        // FELIX  Even break this down into only waht specific StepsFactory needs!
        protected readonly ITutorialArgs _tutorialArgs;

        protected TutorialFactoryBase(ITutorialStepArgsFactory argsFactory, ITutorialArgs tutorialArgs)
        {
            Helper.AssertIsNotNull(argsFactory, tutorialArgs);

            _argsFactory = argsFactory;
            _tutorialArgs = tutorialArgs;
        }
   }
}