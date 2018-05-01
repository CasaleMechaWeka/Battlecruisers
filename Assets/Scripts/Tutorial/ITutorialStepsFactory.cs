using System.Collections.Generic;
using BattleCruisers.Tutorial.Steps;

namespace BattleCruisers.Tutorial
{
    public interface ITutorialStepsFactory
    {
        Queue<ITutorialStep> CreateTutorialSteps();
    }
}
