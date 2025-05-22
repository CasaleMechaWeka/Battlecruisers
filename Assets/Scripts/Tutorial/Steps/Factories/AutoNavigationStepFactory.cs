using BattleCruisers.UI.Cameras;
using BattleCruisers.Utils;
using System.Collections.Generic;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class AutoNavigationStepFactory : TutorialFactoryBase
    {
        private readonly CameraAdjustmentWaitStepFactory _cameraAdjustmentWaitStepFactory;
        private readonly CameraComponents _cameraComponents;

        public AutoNavigationStepFactory(
            TutorialStepArgsFactory argsFactory,
            CameraAdjustmentWaitStepFactory cameraAdjustmentWaitStepFactory,
            CameraComponents cameraComponents)
            : base(argsFactory)
        {
            Helper.AssertIsNotNull(cameraAdjustmentWaitStepFactory, cameraComponents);

            _cameraAdjustmentWaitStepFactory = cameraAdjustmentWaitStepFactory;
            _cameraComponents = cameraComponents;
        }

        public IList<TutorialStep> CreateSteps(CameraFocuserTarget cameraFocuserTarget)
        {
            IList<TutorialStep> steps = new List<TutorialStep>();

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