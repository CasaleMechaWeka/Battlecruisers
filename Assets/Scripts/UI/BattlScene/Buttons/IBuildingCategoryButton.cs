using BattleCruisers.Buildables.Buildings;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public interface IBuildingCategoryButton : IButton
    {
        bool IsActiveFeedbackVisible { set; }
        BuildingCategory Category { get; }
    }
}
