using System.Collections.Generic;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public interface ITutorialStepsFactory
    {
        IList<ITutorialStep> CreateTutorialSteps();
    }
}
