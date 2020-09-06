using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Offensive;
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
        private readonly ISlidingPanelWaitStepFactory _slidingPanelWaitStepFactory;

        public ConstructBuildingStepsFactory(
            ITutorialStepArgsFactory argsFactory,
            LeftPanelComponents leftPanelComponents,
            ITutorialProvider tutorialProvider,
            ICruiser playerCruiser,
            ISingleBuildableProvider lastPlayerIncompleteBuildingStartedProvider,
            ISlidingPanelWaitStepFactory slidingPanelWaitStepFactory)
            : base(argsFactory)
        {
            Helper.AssertIsNotNull(leftPanelComponents, tutorialProvider, playerCruiser, lastPlayerIncompleteBuildingStartedProvider, slidingPanelWaitStepFactory);

            _leftPanelComponents = leftPanelComponents;
            _tutorialProvider = tutorialProvider;
            _playerCruiser = playerCruiser;
            _lastPlayerIncompleteBuildingStartedProvider = lastPlayerIncompleteBuildingStartedProvider;
            _slidingPanelWaitStepFactory = slidingPanelWaitStepFactory;
        }

        public IList<ITutorialStep> CreateSteps(
            BuildingCategory buildingCategory,
            BuildableInfo buildingToConstruct,
            ISlotSpecification slotSpecification,
            string constructBuildingInstruction,
            bool waitForBuildingToComplete = true)
        {
            IList<ITutorialStep> constructionSteps = new List<ITutorialStep>();

            // Select building category
            IBuildingCategoryButton buildingCategoryButton = _leftPanelComponents.BuildMenu.GetBuildingCategoryButton(buildingCategory);
            Assert.IsNotNull(buildingCategoryButton);
            ITutorialStepArgs buildingCategoryArgs = _argsFactory.CreateTutorialStepArgs(constructBuildingInstruction, buildingCategoryButton, shouldUnhighlight: false);
            constructionSteps.Add(new CategoryButtonStep(buildingCategoryArgs, buildingCategoryButton, _tutorialProvider.BuildingCategoryPermitter));

            // Wait for selector panel to slide out
            constructionSteps.Add(_slidingPanelWaitStepFactory.CreateSelectorShownWaitStep());

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

            ReadOnlyCollection<IBuildableButton> categoryButtons = _leftPanelComponents.BuildMenu.GetBuildingButtons(buildingCategory);

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