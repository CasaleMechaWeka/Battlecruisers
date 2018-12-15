using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class AutoNavigationStepFactory : TutorialFactoryBase, IAutoNavigationStepFactory
    {
        private readonly ITutorialStepFactory _cameraAdjustmentWaitStepFactory;

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
                    _tutorialArgs.CameraComponents.CameraFocuser,
                    cameraFocuserTarget));

            steps.Add(_cameraAdjustmentWaitStepFactory.CreateStep());

            return steps;
        }
    }
}