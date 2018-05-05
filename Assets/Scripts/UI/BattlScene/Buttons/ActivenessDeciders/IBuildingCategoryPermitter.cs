using BattleCruisers.Buildables.Buildings;

namespace BattleCruisers.UI.BattleScene.Buttons.ActivenessDeciders
{
    public interface IBuildingCategoryPermitter
    {
        BuildingCategory? PermittedCategory { set; }
    }
}
