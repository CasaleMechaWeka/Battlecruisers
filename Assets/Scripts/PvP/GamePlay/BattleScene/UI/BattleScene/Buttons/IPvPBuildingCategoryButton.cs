using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons
{
    public interface IPvPBuildingCategoryButton : IPvPButton
    {
        bool IsActiveFeedbackVisible { set; }
        PvPBuildingCategory Category { get; }

        /// <summary>
        /// For hotkeys.
        /// </summary>
        void TriggerClick();
    }
}
