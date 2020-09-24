using BattleCruisers.Buildables.Buildings;

namespace BattleCruisers.UI.BattleScene.Buttons.Filters
{
    public interface IBuildingCategoryPermitter
    {
        void AllowSingleCategory(BuildingCategory buildingCategory);
        void AllowAllCategories();
        void AllowNoCategories();
    }
}
