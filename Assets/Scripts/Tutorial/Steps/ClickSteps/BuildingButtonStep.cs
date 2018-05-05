using System;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Buttons.ActivenessDeciders;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.ClickSteps
{
    // FELIX  Test :)
    public class BuildingButtonStep : ClickStep
    {
        private readonly IBuildingPermitter _permitter;
        private readonly IPrefabKey _buildingToAllow;

        public BuildingButtonStep(
            ITutorialStepArgs args, 
            IBuildableButton completionClickable,
            IBuildingPermitter permitter,
            IPrefabKey buildingToAllow) 
            : base(args, completionClickable)
        {
            Assert.IsNotNull(permitter);

            _permitter = permitter;
            _buildingToAllow = buildingToAllow;
        }

		public override void Start(Action completionCallback)
		{
            base.Start(completionCallback);

            _permitter.PermittedBuilding = _buildingToAllow;
		}

		protected override void OnCompleted()
		{
            _permitter.PermittedBuilding = null;

			base.OnCompleted();
		}
	}
}
