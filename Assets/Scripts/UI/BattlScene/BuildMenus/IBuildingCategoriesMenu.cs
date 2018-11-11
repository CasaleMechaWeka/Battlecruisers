using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.BattleScene.Buttons;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public interface IBuildingCategoriesMenu : IMenu
    {
        IBuildingCategoryButton GetCategoryButton(BuildingCategory category);
    }
}