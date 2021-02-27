using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public abstract class TutorialFactoryBase
    {
        protected readonly ITutorialStepArgsFactory _argsFactory;
        protected readonly ILocTable _tutorialStrings;

        protected TutorialFactoryBase(ITutorialStepArgsFactory argsFactory, ILocTable tutorialStrings)
        {
            Helper.AssertIsNotNull(argsFactory, tutorialStrings);

            _argsFactory = argsFactory;
            _tutorialStrings = tutorialStrings;
        }
   }
}