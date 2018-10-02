using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Tutorial.Steps;
using BattleCruisers.Tutorial.Steps.ClickSteps;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Tutorial.Steps.ClickSteps
{
    public class BuildingButtonStepTests : TutorialStepTestsBase
    {
        private ITutorialStep _clickStep;
        private IBuildableButton _buildableButton;
        private IBuildingPermitter _permitter;
        private IPrefabKey _buildingToAllow;

        [SetUp]
        public override void SetuUp()
        {
            base.SetuUp();

            _buildableButton = Substitute.For<IBuildableButton>();
            _permitter = Substitute.For<IBuildingPermitter>();
            _buildingToAllow = Substitute.For<IPrefabKey>();

            /// FELIX  Update tests
            //_clickStep = new BuildingButtonStep(_args, _buildableButton, _permitter, _buildingToAllow);
        }

        [Test]
        public void Start_PermitsBuilding()
        {
            _clickStep.Start(_completionCallback);
            _permitter.Received().PermittedBuilding = _buildingToAllow;
        }

        [Test]
        public void Click_PermittedBuildingCleared()
        {
            Start_PermitsBuilding();

            _buildableButton.Clicked += Raise.Event();
            _permitter.Received().PermittedBuilding = null;
        }
    }
}
