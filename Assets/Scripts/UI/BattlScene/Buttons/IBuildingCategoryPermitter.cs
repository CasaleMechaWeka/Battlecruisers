using BattleCruisers.Buildables.Buildings;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public interface IBuildingCategoryPermitter
    {
        BuildingCategory? PermittedCategory { set; }
    }
}
