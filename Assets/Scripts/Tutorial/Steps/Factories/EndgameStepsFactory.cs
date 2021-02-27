using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Cruisers;
using BattleCruisers.Tutorial.Providers;
using BattleCruisers.Tutorial.Steps.BoostSteps;
using BattleCruisers.Tutorial.Steps.WaitSteps;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using System.Collections.Generic;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class EndgameStepsFactory : TutorialFactoryBase, ITutorialStepsFactory
    {
        private readonly IChangeCruiserBuildSpeedStepFactory _changeCruiserBuildSpeedStepFactory;
        private readonly IAutoNavigationStepFactory _autoNavigationStepFactory;
        private readonly ITutorialProvider _tutorialProvider;
        private readonly ICruiser _playerCruiser, _aiCruiser;

        public EndgameStepsFactory(
            ITutorialStepArgsFactory argsFactory,
            ILocTable tutorialStrings,
            IChangeCruiserBuildSpeedStepFactory changeCruiserBuildSpeedStepFactory, 
            IAutoNavigationStepFactory autoNavigationStepFactory,
            ITutorialProvider tutorialProvider, 
            ICruiser playerCruiser, 
            ICruiser aiCruiser)
            : base(argsFactory, tutorialStrings)
        {
            Helper.AssertIsNotNull(changeCruiserBuildSpeedStepFactory, autoNavigationStepFactory, tutorialProvider, playerCruiser, aiCruiser);

            _changeCruiserBuildSpeedStepFactory = changeCruiserBuildSpeedStepFactory;
            _autoNavigationStepFactory = autoNavigationStepFactory;
            _tutorialProvider = tutorialProvider;
            _playerCruiser = playerCruiser;
            _aiCruiser = aiCruiser;
        }

        public IList<ITutorialStep> CreateSteps()
        {
            List<ITutorialStep> steps = new List<ITutorialStep>();

            // Return build speed to normal
            steps.Add(
                _changeCruiserBuildSpeedStepFactory.CreateStep(
                    _tutorialProvider.PlayerCruiserBuildSpeedController,
                    BuildSpeed.Normal));

            // Navigate to player cruiser
            steps.AddRange(_autoNavigationStepFactory.CreateSteps(CameraFocuserTarget.PlayerCruiser));

            // FELIX  Loc
            // Wait for artillery to complete
            steps.Add(
                new BuildableCompletedWaitStep(
                    _argsFactory.CreateTutorialStepArgs("Wait for your artillery to complete.  Patience :)"),
                    _tutorialProvider.SingleOffensiveProvider));

            // Boost artillery accuracy and fire rate, so that enemy cruiser is destroyed more quickly :)
            steps.AddRange(CreateSteps_BoostArtillery());

            // Zoom out so user can see artillery firing
            steps.AddRange(_autoNavigationStepFactory.CreateSteps(CameraFocuserTarget.Overview));

            // Wait for enemy cruiser to be destroyed
            steps.Add(
                new TargetDestroyedWaitStep(
                    _argsFactory.CreateTutorialStepArgs("Done!  Your Artillery will bombard the enemy on auto-pilot.  Feel free to look around!"),
                    new StaticProvider<ITarget>(_aiCruiser)));

            return steps;
        }

        private IList<ITutorialStep> CreateSteps_BoostArtillery()
        {
            return new List<ITutorialStep>()
            {
                new AddTurretAccuracyBoostStep(
                    _argsFactory.CreateTutorialStepArgs(),
                    _playerCruiser.CruiserSpecificFactories.GlobalBoostProviders,
                    // 0.05 * 20 = 1 (100% accuracy)
                    new BoostProvider(20)),

                new AddArtilleryFireRateBoostStep(
                    _argsFactory.CreateTutorialStepArgs(),
                    _playerCruiser.CruiserSpecificFactories.GlobalBoostProviders,
                    new BoostProvider(3))
            };
        }
    }
}