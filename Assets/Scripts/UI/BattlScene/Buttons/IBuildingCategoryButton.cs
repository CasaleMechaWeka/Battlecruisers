using BattleCruisers.Buildables.Buildings;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public interface IBuildingCategoryButton : IButton
    {
        BuildingCategory Category { get; }
    }
}
