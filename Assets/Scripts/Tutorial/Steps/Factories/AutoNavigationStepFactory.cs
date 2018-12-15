using BattleCruisers.UI.Cameras;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class AutoNavigationStepFactory : TutorialFactoryBase, IAutoNavigationStepFactory
    {
        private readonly ITutorialStepFactory _cameraAdjustmentWaitStepFactory;
        private readonly ICameraComponents _cameraComponents;

        public AutoNavigationStepFactory(
            ITutorialStepArgsFactory argsFactory, 
            ITutorialArgs tutorialArgs,
            ITutorialStepFactory cameraAdjustmentWaitStepFactory) 
            : base(argsFactory, tutorialArgs)
        {
            Assert.IsNotNull(cameraAdjustmentWaitStepFactory);
            _cameraAdjustmentWaitStepFactory = cameraAdjustmentWaitStepFactory;
        }

        public IList<ITutorialStep> CreateSteps(CameraFocuserTarget cameraFocuserTarget)
        {
            IList<ITutorialStep> steps = new List<ITutorialStep>();

            steps.Add(
                new CameraFocuserStep(
                    _argsFactory.CreateTutorialStepArgs(),
                    _cameraComponents.CameraFocuser,
                    cameraFocuserTarget));

            steps.Add(_cameraAdjustmentWaitStepFactory.CreateStep());

            return steps;
        }
    }
}