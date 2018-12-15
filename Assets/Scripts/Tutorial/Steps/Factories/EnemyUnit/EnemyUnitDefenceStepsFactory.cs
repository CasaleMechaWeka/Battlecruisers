using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Tutorial.Steps.EnemyCruiser;
using BattleCruisers.Tutorial.Steps.Providers;
using BattleCruisers.Tutorial.Steps.WaitSteps;
using BattleCruisers.Utils.Strings;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.Factories.EnemyUnit
{
    public abstract class EnemyUnitDefenceStepsFactory : TutorialFactoryBase, ITutorialStepsFactory
    {
        private readonly ICreateProducingFactoryStepsFactory _createProducingFactoryStepsFactory;
        private readonly IAutoNavigationStepFactory _autoNavigationStepFactory;
        private readonly IExplanationDismissableStepFactory _explanationDismissableStepFactory;
        private readonly IConstructBuildingStepsFactory _constructBuildingStepsFactory;
        private readonly IChangeCruiserBuildSpeedStepFactory _changeCruiserBuildSpeedStepFactory;
        private readonly ITutorialProvider _tutorialProvider;

        protected abstract IPrefabKey FactoryKey { get; }
        protected abstract BuildableInfo UnitToBuild { get; }
        protected abstract ISingleBuildableProvider UnitBuiltProvider { get; }
        protected abstract BuildableInfo DefenceToBuild { get; }
        protected abstract SlotSpecification SlotSpecification { get; }
        protected abstract CameraFocuserTarget UnitCameraFocusTarget { get; }

        public EnemyUnitDefenceStepsFactory(
            ITutorialStepArgsFactory argsFactory,
            ITutorialArgs tutorialArgs,
            EnemyUnitArgs enemyUnitArgs) 
            : base(argsFactory, tutorialArgs)
        {
            Assert.IsNotNull(enemyUnitArgs);

            _createProducingFactoryStepsFactory = enemyUnitArgs.CreateProducingFactoryStepsFactory;
            _autoNavigationStepFactory = enemyUnitArgs.AutoNavigationStepFactory;
            _explanationDismissableStepFactory = enemyUnitArgs.ExplanationDismissableStepFactory;
            _constructBuildingStepsFactory = enemyUnitArgs.ConstructBuildingStepsFactory;
            _changeCruiserBuildSpeedStepFactory = enemyUnitArgs.ChangeCruiserBuildSpeedStepFactory;
            _tutorialProvider = enemyUnitArgs.TutorialProvider;
        }

        public IList<ITutorialStep> CreateTutorialSteps()
        {
            List<ITutorialStep> enemyUnitDefenceSteps = new List<ITutorialStep>();

            // 1. Create factory and start producing units
            FactoryStepsResult factoryStepsResult = _createProducingFactoryStepsFactory.CreateSteps(FactoryKey, UnitToBuild.Key);
            enemyUnitDefenceSteps.AddRange(factoryStepsResult.Steps);

            // 2. Navigate to enemey cruiser
            enemyUnitDefenceSteps.AddRange(_autoNavigationStepFactory.CreateSteps(UnitCameraFocusTarget));

            // 3. Acknowledge the unit
            string indefiniteArticle = IndefiniteyArticleHelper.FindIndefiniteArticle(UnitToBuild.Name);
            string textToDisplay = "Uh oh, the enemy is building " + indefiniteArticle + " " + UnitToBuild.Name + "!";
            ITutorialStepArgs clickUnitArgs = _argsFactory.CreateTutorialStepArgs(textToDisplay, UnitBuiltProvider);
            enemyUnitDefenceSteps.Add(_explanationDismissableStepFactory.CreateTutorialStep(clickUnitArgs));

            // 4. Navigate back to player cruiser
            enemyUnitDefenceSteps.AddRange(_autoNavigationStepFactory.CreateSteps(CameraFocuserTarget.PlayerCruiser));

            // 5. Build defence turret
            IList<ITutorialStep> buildTurretSteps
                = _constructBuildingStepsFactory.CreateSteps(
                    BuildingCategory.Defence,
                    DefenceToBuild,
                    SlotSpecification,
                    "Quick, build an " + DefenceToBuild.Name + "!");
            enemyUnitDefenceSteps.AddRange(buildTurretSteps);

            // 6. Navigate to mid left
            enemyUnitDefenceSteps.AddRange(_autoNavigationStepFactory.CreateSteps(CameraFocuserTarget.MidLeft));

            // 7. Insta-complete unit
            enemyUnitDefenceSteps.Add(
                _changeCruiserBuildSpeedStepFactory.CreateTutorialStep(
                    _tutorialProvider.AICruiserBuildSpeedController,
                    BuildSpeed.VeryFast));

            enemyUnitDefenceSteps.Add(
                new BuildableCompletedWaitStep(
                    _argsFactory.CreateTutorialStepArgs(),
                    UnitBuiltProvider));

            enemyUnitDefenceSteps.Add(
                new StopUnitConstructionStep(
                    _argsFactory.CreateTutorialStepArgs(),
                    factoryStepsResult.FactoryProvider));

            string unitComingText = "Here comes the enemy " + UnitToBuild.Name + ".";

            // 7.5  Optionally boost unit speed until just before it reaches the user's camera view
            enemyUnitDefenceSteps.AddRange(CreateSpeedBoostSteps(unitComingText));

            // 8. Wait for defence turret to destroy unit
            enemyUnitDefenceSteps.Add(
                new TargetDestroyedWaitStep(
                    _argsFactory.CreateTutorialStepArgs(unitComingText),
                    new BuildableToTargetProvider(UnitBuiltProvider)));

            // 9. Congrats!
            enemyUnitDefenceSteps.Add(
                _explanationDismissableStepFactory.CreateTutorialStep(
                    _argsFactory.CreateTutorialStepArgs("Nice!  You have successfully defended your cruiser.")));

            return enemyUnitDefenceSteps;
        }

        protected virtual IList<ITutorialStep> CreateSpeedBoostSteps(string unitUpcomingText)
        {
            return new List<ITutorialStep>();
        }
    }
}