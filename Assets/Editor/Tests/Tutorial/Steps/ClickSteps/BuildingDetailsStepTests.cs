// FELIX  Convert or delete :)
//using System.Collections.Generic;
//using BattleCruisers.Tutorial.Providers;
//using BattleCruisers.Tutorial.Steps;
//using BattleCruisers.Tutorial.Steps.ClickSteps;
//using BattleCruisers.Tutorial.Steps.Providers;
//using BattleCruisers.UI.BattleScene.Manager;
//using NSubstitute;
//using NUnit.Framework;

//namespace BattleCruisers.Tests.Tutorial.Steps.ClickSteps
//{
//    public class BuildingDetailsStepTests : TutorialStepTestsBase
//    {
//        private ITutorialStep _clickStep;
//        private ISingleBuildableProvider _buildableProvider;
//        private IListProvider<IClickableEmitter> _clickablesProvider;
//        private IUIManagerSettablePermissions _uiManagerPermissions;
//        private IClickableEmitter _building;

//        [SetUp]
//        public override void SetuUp()
//        {
//            base.SetuUp();

//            _buildableProvider = Substitute.For<ISingleBuildableProvider>();
//            _clickablesProvider = _buildableProvider;
//            _building = Substitute.For<IClickableEmitter>();
//            IList<IClickableEmitter> clickables = new List<IClickableEmitter>()
//            {
//                _building
//            };
//            _clickablesProvider.FindItems().Returns(clickables);

//            _uiManagerPermissions = Substitute.For<IUIManagerSettablePermissions>();

//            _clickStep = new BuildingDetailsStep(_args, _buildableProvider, _uiManagerPermissions);
//        }

//        [Test]
//        public void Start_EnablesBuildableDetails()
//        {
//            _clickStep.Start(_completionCallback);
//            _uiManagerPermissions.Received().CanShowItemDetails = true;
//        }

//        [Test]
//        public void Click_DisablesBuildableDetails()
//        {
//            Start_EnablesBuildableDetails();

//            _building.Clicked += Raise.Event();
//            _uiManagerPermissions.Received().CanShowItemDetails = false;
//        }
//    }
//}
