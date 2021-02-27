using BattleCruisers.Tutorial.Steps.WaitSteps;
using BattleCruisers.UI.Cameras;
using BattleCruisers.Utils.Localisation;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class CameraAdjustmentWaitStepFactory : TutorialFactoryBase, ITutorialStepFactory
    {
        private readonly ICameraComponents _cameraComponents;

        public CameraAdjustmentWaitStepFactory(
            ITutorialStepArgsFactory argsFactory, 
            ILocTable tutorialStrings,
            ICameraComponents cameraComponents)
            : base(argsFactory, tutorialStrings)
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