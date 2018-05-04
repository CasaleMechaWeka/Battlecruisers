using System;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.BattleScene.Buttons;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps
{
    public class CategoryButtonStep : ClickStep
    {
        private readonly IBuildingCategoryPermitter _permitter;
        private readonly BuildingCategory _category;

        public CategoryButtonStep(
            ITutorialStepArgs args, 
            IBuildingCategoryButton buildingCategoryButton,
            IBuildingCategoryPermitter permitter) 
            : base(args, buildingCategoryButton)
        {
            Assert.IsNotNull(permitter);

            _permitter = permitter;
            _category = buildingCategoryButton.Category;
        }

		public override void Start(Action completionCallback)
		{
            base.Start(completionCallback);

            _permitter.PermittedCategory = _category;
		}

		protected override void OnCompleted()
		{
            _permitter.PermittedCategory = null;

			base.OnCompleted();
		}
	}
}
