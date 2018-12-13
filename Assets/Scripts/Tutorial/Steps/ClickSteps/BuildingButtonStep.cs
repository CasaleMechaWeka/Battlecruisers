using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Tutorial.Providers;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.Tutorial.Steps.ClickSteps
{
    /// <summary>
    /// Must be used in tandem with SlotsStep.
    /// </summary>
    public class BuildingButtonStep : ExplanationClickStep
    {
        private readonly IBuildingPermitter _buildingPermitter;
        private readonly IPrefabKey _buildingToAllow;
        private readonly IItemProvider<ISlot> _slotProvider;
        private readonly ISlotPermitter _highlightableSlotPermitter;

        public BuildingButtonStep(
            ITutorialStepArgsNEW args, 
            IBuildableButton buildableButton,
            IBuildingPermitter buildingPermitter,
            IPrefabKey buildingToAllow,
            IItemProvider<ISlot> slotProvider,
            ISlotPermitter highlightableSlotPermitter) 
            : base(args, new StaticProvider<IClickableEmitter>(buildableButton))
        {
            Helper.AssertIsNotNull(buildingPermitter, buildingToAllow, slotProvider, highlightableSlotPermitter);

            _buildingPermitter = buildingPermitter;
            _buildingToAllow = buildingToAllow;
            _slotProvider = slotProvider;
            _highlightableSlotPermitter = highlightableSlotPermitter;
        }

		public override void Start(Action completionCallback)
		{
            base.Start(completionCallback);

            _buildingPermitter.PermittedBuilding = _buildingToAllow;

            // This was previously in SlotsStep.Start(), but clicking the buliding button
            // and highlighting slots happens at the same time, hence need to set both
            // permitters at the same time.
            _highlightableSlotPermitter.PermittedSlot = _slotProvider.FindItem();
		}

		protected override void OnCompleted()
		{
            _buildingPermitter.PermittedBuilding = null;

			base.OnCompleted();
		}
	}
}
