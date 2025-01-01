using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.BuildMenus
{
    public interface IPvPBuildingCategoriesMenu : IPvPMenu
    {
        IPvPBuildingCategoryButton GetCategoryButton(BuildingCategory category);
    }
}