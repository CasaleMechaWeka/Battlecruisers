using BattleCruisers.Buildables.Buildings;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons
{
    public interface IPvPBuildingCategoryButton : IPvPButton
    {
        bool IsActiveFeedbackVisible { set; }
        BuildingCategory Category { get; }

        /// <summary>
        /// For hotkeys.
        /// </summary>
        void TriggerClick();
    }
}
