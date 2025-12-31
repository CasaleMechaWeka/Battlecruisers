using BattleCruisers.Utils;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public abstract class TutorialFactoryBase
    {
        protected readonly TutorialStepArgsFactory _argsFactory;

        protected TutorialFactoryBase(TutorialStepArgsFactory argsFactory)
        {
            Helper.AssertIsNotNull(argsFactory);

            _argsFactory = argsFactory;
        }
    }
}