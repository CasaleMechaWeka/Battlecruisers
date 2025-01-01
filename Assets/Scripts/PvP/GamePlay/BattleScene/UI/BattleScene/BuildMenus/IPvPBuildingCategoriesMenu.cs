using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.BattleScene.Buttons;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.BuildMenus
{
    public interface IPvPBuildingCategoriesMenu : IPvPMenu
    {
        IBuildingCategoryButton GetCategoryButton(BuildingCategory category);
    }
}