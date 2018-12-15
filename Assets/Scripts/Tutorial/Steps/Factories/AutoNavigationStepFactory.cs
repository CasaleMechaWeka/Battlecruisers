using BattleCruisers.UI.Cameras;
using BattleCruisers.Utils;
using System.Collections.Generic;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class AutoNavigationStepFactory : TutorialFactoryBase, IAutoNavigationStepFactory
    {
        private readonly ITutorialStepFactory _cameraAdjustmentWaitStepFactory;
        private readonly ICameraComponents _cameraComponents;

        public AutoNavigationStepFactory(
            ITutorialStepArgsFactory argsFactory, 
            ITutorialStepFactory cameraAdjustmentWaitStepFactory,
            ICameraComponents cameraComponents) 
            : base(argsFactory)
        {
            Helper.AssertIsNotNull(cameraAdjustmentWaitStepFactory, cameraComponents);

            _cameraAdjustmentWaitStepFactory = cameraAdjustmentWaitStepFactory;
            _cameraComponents = cameraComponents;
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