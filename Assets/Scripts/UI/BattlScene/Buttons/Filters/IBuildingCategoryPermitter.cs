using BattleCruisers.Buildables.Buildings;

namespace BattleCruisers.UI.BattleScene.Buttons.Filters
{
    public interface IBuildingCategoryPermitter
    {
        BuildingCategory? PermittedCategory { set; }
    }
}
