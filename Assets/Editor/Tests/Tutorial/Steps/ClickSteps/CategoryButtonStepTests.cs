using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Tutorial.Steps;
using BattleCruisers.Tutorial.Steps.ClickSteps;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Tutorial.Steps.ClickSteps
{
    public class CategoryButtonStepTests : TutorialStepTestsBase
    {
        private ITutorialStep _clickStep;
        private IBuildingCategoryButton _buildingCategoryButton;
        private IBuildingCategoryPermitter _permitter;

        [SetUp]
        public override void SetuUp()
        {
            base.SetuUp();

            _buildingCategoryButton = Substitute.For<IBuildingCategoryButton>();
            _buildingCategoryButton.Category.Returns(BuildingCategory.Ultra);
            _permitter = Substitute.For<IBuildingCategoryPermitter>();

            _clickStep = new CategoryButtonStepNEW(_args, _buildingCategoryButton, _permitter);
        }

        [Test]
        public void Start_PermitsCategory()
        {
            _clickStep.Start(_completionCallback);
            _permitter.Received().PermittedCategory = _buildingCategoryButton.Category;
        }

        [Test]
        public void Click_PermittedBuildingCleared()
        {
            Start_PermitsCategory();

            _buildingCategoryButton.Clicked += Raise.Event();
            _permitter.Received().PermittedCategory = null;
        }
    }
}
