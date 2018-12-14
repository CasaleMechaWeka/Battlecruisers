using System.Collections.Generic;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public interface IAutoNavigationStepFactory
    {
        IList<ITutorialStep> CreateSteps(CameraFocuserTarget cameraFocuserTarget);
    }
}