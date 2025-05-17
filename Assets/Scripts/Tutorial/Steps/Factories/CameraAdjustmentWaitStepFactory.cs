using BattleCruisers.Tutorial.Steps.WaitSteps;
using BattleCruisers.UI.Cameras;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class CameraAdjustmentWaitStepFactory : TutorialFactoryBase
    {
        private readonly CameraComponents _cameraComponents;

        public CameraAdjustmentWaitStepFactory(
            TutorialStepArgsFactory argsFactory,
            CameraComponents cameraComponents)
            : base(argsFactory)
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