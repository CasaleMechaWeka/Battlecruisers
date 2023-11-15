using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters
{
    public interface IPvPBuildingCategoryPermitter
    {
        void AllowSingleCategory(PvPBuildingCategory buildingCategory);
        void AllowAllCategories();
        void AllowNoCategories();
    }
}
