using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Tutorial.Steps.ClickSteps;
using BattleCruisers.Tutorial.Steps.Providers;
using BattleCruisers.Tutorial.Steps.WaitSteps;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class ConstructBuildingStepsFactory : TutorialFactoryBase, IConstructBuildingStepsFactory
    {
        private readonly LeftPanelComponents _leftPanelComponents;
        private readonly ITutorialProvider _tutorialProvider;
        private readonly ICruiser _playerCruiser;
        private readonly ISingleBuildableProvider _lastPlayerIncompleteBuildingStartedProvider;

        public ConstructBuildingStepsFactory(
            ITutorialStepArgsFactory argsFactory,
            ITutorialArgs tutorialArgs,
            LeftPanelComponents leftPanelComponents,
            ITutorialProvider tutorialProvider,
            ICruiser playerCruiser,
            ISingleBuildableProvider lastPlayerIncompleteBuildingStartedProvider)
            : base(argsFactory, tutorialArgs)
        {
            Helper.AssertIsNotNull(leftPanelComponents, tutorialProvider, playerCruiser, lastPlayerIncompleteBuildingStartedProvider);

            _leftPanelComponents = leftPanelComponents;
            _tutorialProvider = tutorialProvider;
            _playerCruiser = playerCruiser;
            _lastPlayerIncompleteBuildingStartedProvider = lastPlayerIncompleteBuildingStartedProvider;
        }

        public IList<ITutorialStep> CreateSteps(
            BuildingCategory buildingCategory,
            BuildableInfo buildingToConstruct,
            SlotSpecification slotSpecification,
            string constructBuildingInstruction,
            bool waitForBuildingToComplete = true)
        {
            IList<ITutorialStep> constructionSteps = new List<ITutorialStep>();

            // Select building category
            IBuildingCategoryButton buildingCategoryButton = _leftPanelComponents.BuildMenu.GetCategoryButton(buildingCategory);
            Assert.IsNotNull(buildingCategoryButton);
            ITutorialStepArgs buildingCategoryArgs = _argsFactory.CreateTutorialStepArgs(constructBuildingInstruction, buildingCategoryButton);
            constructionSteps.Add(new CategoryButtonStep(buildingCategoryArgs, buildingCategoryButton, _tutorialProvider.BuildingCategoryPermitter));

            // Select building
            IBuildableButton buildingButton = FindBuildableButton(buildingCategory, buildingToConstruct.Key);
            string textToDisplay = null;  // Means previous text is displayed
            ITutorialStepArgs buldingButtonArgs = _argsFactory.CreateTutorialStepArgs(textToDisplay, buildingButton);
            ISlotProvider slotProvider = new SlotProvider(_playerCruiser.SlotAccessor, slotSpecification);
            constructionSteps.Add(
                new BuildingButtonStep(
                    buldingButtonArgs,
                    buildingButton,
                    _tutorialProvider.BuildingPermitter,
                    buildingToConstruct.Key,
                    slotProvider,
                    _tutorialProvider.SlotPermitter));

            // Select a slot
            ITutorialStepArgs buildingSlotsArgs = _argsFactory.CreateTutorialStepArgs(textToDisplay, slotProvider);
            constructionSteps.Add(
                new SlotStep(
                    buildingSlotsArgs,
                    _tutorialProvider.SlotPermitter,
                    slotProvider));

            if (waitForBuildingToComplete)
            {
                // Wait for building to complete construction
                string waitText = "Wait for " + buildingToConstruct.Name + " to complete.  Patience :)";
                constructionSteps.Add(CreateStep_WaitForLastIncomlpeteBuildingToComplete(waitText));
            }

            return constructionSteps;
        }

        private IBuildableButton FindBuildableButton(BuildingCategory buildingCategory, IPrefabKey buildingKey)
        {
            _tutorialProvider.BuildingPermitter.PermittedBuilding = buildingKey;

            ReadOnlyCollection<IBuildableButton> categoryButtons = _leftPanelComponents.BuildMenu.GetBuildableButtons(buildingCategory);

            IBuildableButton buildableButton
                = categoryButtons
                    .FirstOrDefault(button => _tutorialProvider.ShouldBuildingBeEnabledFilter.IsMatch(button.Buildable));

            _tutorialProvider.BuildingPermitter.PermittedBuilding = null;

            Assert.IsNotNull(buildableButton);
            return buildableButton;
        }

        private ITutorialStep CreateStep_WaitForLastIncomlpeteBuildingToComplete(string textToDisplay)
        {
            ITutorialStepArgs args = _argsFactory.CreateTutorialStepArgs(textToDisplay);
            return new BuildableCompletedWaitStep(args, _lastPlayerIncompleteBuildingStartedProvider);
        }
    }
}