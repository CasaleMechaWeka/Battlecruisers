using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.ClickSteps
{
    public class CategoryButtonStep : ExplanationClickStep
    {
        private readonly BuildingCategoryFilter _permitter;
        private readonly BuildingCategory _category;

        public CategoryButtonStep(
            TutorialStepArgs args,
            IBuildingCategoryButton buildingCategoryButton,
            BuildingCategoryFilter permitter)
            : base(args, buildingCategoryButton)
        {
            Assert.IsNotNull(permitter);

            _permitter = permitter;
            _category = buildingCategoryButton.Category;
        }

        public override void Start(Action completionCallback)
        {
            base.Start(completionCallback);
            _permitter.AllowSingleCategory(_category);
        }

        protected override void OnCompleted()
        {
            _permitter.AllowNoCategories();
            base.OnCompleted();
        }
    }
}
