using BattleCruisers.Tutorial.Steps.WaitSteps;
using BattleCruisers.UI.Cameras;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class CameraAdjustmentWaitStepFactory : TutorialFactoryBase, ITutorialStepFactory
    {
        private readonly ICameraComponents _cameraComponents;

        public CameraAdjustmentWaitStepFactory(
            ITutorialStepArgsFactory argsFactory, 
            ITutorialArgs tutorialArgs,
            ICameraComponents cameraComponents) 
            : base(argsFactory, tutorialArgs)
        {
            Assert.IsNotNull(cameraComponents);
            _cameraComponents = cameraComponents;
        }

        public ITutorialStep CreateStep()
        {
            return
                new CameraAdjustmentWaitStep(
                    _argsFactory.CreateTutorialStepArgs(),
                    _cameraComponents.CameraAdjuster);
        }
    }
}