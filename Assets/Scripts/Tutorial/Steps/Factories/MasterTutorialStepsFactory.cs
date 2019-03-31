using BattleCruisers.Tutorial.Explanation;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.Threading;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class MasterTutorialStepsFactory : ITutorialStepsFactory
    {
        private readonly TutorialStepsFactoriesProvider _factoriesProvider;
        private readonly ISystemInfo _systemInfo;

        public MasterTutorialStepsFactory(
            IHighlighter highlighter,
            IExplanationPanel explanationPanel,
            IDeferrer deferrer,
            ITutorialArgs tutorialArgs)
        {
            Helper.AssertIsNotNull(highlighter, explanationPanel, deferrer, tutorialArgs);

            _factoriesProvider
                = new TutorialStepsFactoriesProvider(
                    highlighter,
                    explanationPanel,
                    deferrer,
                    tutorialArgs);
            _systemInfo = new SystemInfoBC();
        }

        public IList<ITutorialStep> CreateSteps()
        {
            List<ITutorialStep> steps = new List<ITutorialStep>();

            // 1. Player cruiser
            steps.AddRange(_factoriesProvider.YourCruiserStepsFactory.CreateSteps());

            // 2. Navigation wheel
            steps.AddRange(_factoriesProvider.NavigationWheelStepsFactory.CreateSteps());

            // 2.5 Scroll wheel
            if (_systemInfo.DeviceType != DeviceType.Handheld)
            {
                steps.AddRange(_factoriesProvider.ScrollWheelStepsFactory.CreateSteps());
            }

            // 3. Enemy cruiser
            steps.AddRange(_factoriesProvider.EnemyCruiserStepsFactory.CreateSteps());

            // 4. Player cruiser widgets
            steps.AddRange(_factoriesProvider.PlayerCruiserWidgetsStepsFactory.CreateSteps());

            // 5. Construct drone station
            steps.AddRange(_factoriesProvider.ConstructDroneStationStepsFactory.CreateSteps());

            // 6. Enemy ship
            steps.AddRange(_factoriesProvider.EnemyShipStepsFactory.CreateSteps());

            // 7. Enemy bomber
            steps.AddRange(_factoriesProvider.EnemyBomberStepsFactory.CreateSteps());

            // 8. Drone focus
            steps.AddRange(_factoriesProvider.DroneFocusStepsFactory.CreateSteps());

            // 9. Game speed
            steps.AddRange(_factoriesProvider.GameSpeedStepsFactory.CreateSteps());

            // 10. Endgame
            steps.AddRange(_factoriesProvider.EndgameStepsFactory.CreateSteps());

            return steps;
        }
    }
}