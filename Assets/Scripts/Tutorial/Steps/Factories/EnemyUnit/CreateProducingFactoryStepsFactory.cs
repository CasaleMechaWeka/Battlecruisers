using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Tutorial.Providers;
using BattleCruisers.Tutorial.Steps.EnemyCruiser;
using BattleCruisers.Tutorial.Steps.Providers;
using BattleCruisers.Tutorial.Steps.WaitSteps;
using BattleCruisers.Utils;
using System.Collections.Generic;

namespace BattleCruisers.Tutorial.Steps.Factories.EnemyUnit
{
    public class CreateProducingFactoryStepsFactory : TutorialFactoryBase
    {
        private readonly ChangeCruiserBuildSpeedStepFactory _changeCruiserBuildSpeedStepFactory;
        private readonly ITutorialProvider _tutorialProvider;
        private readonly ICruiser _aiCruiser;

        public CreateProducingFactoryStepsFactory(
            TutorialStepArgsFactory argsFactory,
            ChangeCruiserBuildSpeedStepFactory changeCruiserBuildSpeedStepFactory,
            ITutorialProvider tutorialProvider,
            ICruiser aiCruiser)
            : base(argsFactory)
        {
            Helper.AssertIsNotNull(changeCruiserBuildSpeedStepFactory, tutorialProvider, aiCruiser);

            _changeCruiserBuildSpeedStepFactory = changeCruiserBuildSpeedStepFactory;
            _tutorialProvider = tutorialProvider;
            _aiCruiser = aiCruiser;
        }

        public (IList<TutorialStep> Steps, IItemProvider<IFactory> FactoryProvider) CreateSteps(IPrefabKey factoryKey, IPrefabKey unitKey)
        {
            IList<TutorialStep> factorySteps = new List<TutorialStep>();

            // These steps should complete very quickly and require no user input.
            // There is no need to display any text to the user or highlight any
            // elements.
            TutorialStepArgs commonArgs = _argsFactory.CreateTutorialStepArgs();

            // 1. Change build speed to super fast
            factorySteps.Add(_changeCruiserBuildSpeedStepFactory.CreateStep(
                    _tutorialProvider.AICruiserBuildSpeedController,
                    BuildSpeed.VeryFast));

            // 2. Start building factory
            StartConstructingBuildingStep startConstructingFactoryStep
                = new StartConstructingBuildingStep(
                    commonArgs,
                    factoryKey,
                    _aiCruiser);
            factorySteps.Add(startConstructingFactoryStep);

            // 3. Wait for factory completion
            factorySteps.Add(new BuildableCompletedWaitStep(commonArgs, startConstructingFactoryStep));

            // 4. Change build speed to infinitely slow
            factorySteps.Add(
                _changeCruiserBuildSpeedStepFactory.CreateStep(
                    _tutorialProvider.AICruiserBuildSpeedController,
                    BuildSpeed.InfinitelySlow));

            // 5. Start building unit
            IItemProvider<IFactory> factoryProvider = new BuildableToFactoryProvider(startConstructingFactoryStep);
            factorySteps.Add(
                new StartConstructingUnitStep(
                    commonArgs,
                    unitKey,
                    factoryProvider));

            return new(factorySteps, factoryProvider);
        }
    }
}