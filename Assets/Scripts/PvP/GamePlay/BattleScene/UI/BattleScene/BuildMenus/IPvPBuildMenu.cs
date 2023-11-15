using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Panels;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.BuildMenus
{
    public interface IPvPBuildMenu : IPvPBuildMenuButtons
    {
        IPvPSlidingPanel SelectorPanel { get; }

        /// <summary>
        /// Shows the building group menu for the given buildingCategory.
        /// Hides any currently shown menu.
        /// </summary>
        void ShowBuildingGroupMenu(PvPBuildingCategory buildingCategory);

        /// <summary>
        /// Shows the units menu for the given factory.  Hides any currently 
        /// shown menu.
        /// </summary>
        void ShowUnitsMenu(IPvPFactory factory);

        /// <summary>
        /// Hides the currently shown menu, if there is one.
        /// </summary>
        void HideCurrentlyShownMenu();
    }
}
