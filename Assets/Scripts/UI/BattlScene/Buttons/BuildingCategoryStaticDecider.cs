using BattleCruisers.Buildables.Buildings;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public class BuildingCategoryStaticDecider : StaticDecider<BuildingCategory>
    {
        public BuildingCategoryStaticDecider(bool shouldBeEnabled) 
            : base(shouldBeEnabled)
        {
        }
    }
}
