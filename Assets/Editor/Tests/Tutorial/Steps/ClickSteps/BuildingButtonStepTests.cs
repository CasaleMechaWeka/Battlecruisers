using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Tutorial.Providers;
using BattleCruisers.Tutorial.Steps;
using BattleCruisers.Tutorial.Steps.ClickSteps;
using BattleCruisers.Tutorial.Steps.Providers;
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
        private IBuildingPermitter _buildingPermitter;
        private IPrefabKey _buildingToAllow;
        private ISlotPermitter _slotPermitter;
        private ISlot _slot1, _slot2;
        private ISlot[] _slots;
        private ISlotsProvider _slotsProvider;
        private IListProvider<ISlot> _explicitSlotsProvider;

        [SetUp]
        public override void SetuUp()
        {
            base.SetuUp();

            _buildableButton = Substitute.For<IBuildableButton>();
            _buildingPermitter = Substitute.For<IBuildingPermitter>();
            _buildingToAllow = Substitute.For<IPrefabKey>();

            _slotPermitter = Substitute.For<ISlotPermitter>();

            _slot1 = Substitute.For<ISlot>();
            _slot2 = Substitute.For<ISlot>();
            _slots = new ISlot[]
            {
                _slot1,
                _slot2
            };
            _slotsProvider = Substitute.For<ISlotsProvider>();

            _explicitSlotsProvider = _slotsProvider;
            _explicitSlotsProvider.FindItems().Returns(_slots);

            _clickStep = new BuildingButtonStepNEW(_args, _buildableButton, _buildingPermitter, _buildingToAllow, _slotsProvider, _slotPermitter);
        }

        [Test]
        public void Start_PermitsBuilding()
        {
            _clickStep.Start(_completionCallback);

            _buildingPermitter.Received().PermittedBuilding = _buildingToAllow;
            _slotPermitter.Received().PermittedSlots = _slots;
        }

        [Test]
        public void Click_PermittedBuildingCleared()
        {
            Start_PermitsBuilding();
            _slotPermitter.ClearReceivedCalls();

            _buildableButton.Clicked += Raise.Event();

            _buildingPermitter.Received().PermittedBuilding = null;
            _slotPermitter.DidNotReceiveWithAnyArgs().PermittedSlots = null;
        }
    }
}
