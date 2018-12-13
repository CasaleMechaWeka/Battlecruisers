using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public interface IBuildMenu : IBuildMenuButtons
    {
        /// <summary>
        /// Shows the building group menu for the given buildingCategory.
        /// Hides any currently shown menu.
        /// </summary>
        void ShowBuildingGroupMenu(BuildingCategory buildingCategory);

        /// <summary>
        /// Shows the units menu for the given factory.  Hides any currently 
        /// shown menu.
        /// </summary>
        void ShowUnitsMenu(IFactory factory);

        /// <summary>
        /// Hides the currently shown menu, if there is one.
        /// </summary>
        void HideCurrentlyShownMenu();
    }
}
