using BattleCruisers.Utils;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public abstract class TutorialFactoryBase
    {
        protected readonly ITutorialStepArgsFactory _argsFactory;

        protected TutorialFactoryBase(ITutorialStepArgsFactory argsFactory)
        {
            Helper.AssertIsNotNull(argsFactory);
            _argsFactory = argsFactory;
        }
   }
}