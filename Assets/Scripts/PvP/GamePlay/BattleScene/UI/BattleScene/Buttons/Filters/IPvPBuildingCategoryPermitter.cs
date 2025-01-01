using BattleCruisers.Buildables.Buildings;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters
{
    public interface IPvPBuildingCategoryPermitter
    {
        void AllowSingleCategory(BuildingCategory buildingCategory);
        void AllowAllCategories();
        void AllowNoCategories();
    }
}
