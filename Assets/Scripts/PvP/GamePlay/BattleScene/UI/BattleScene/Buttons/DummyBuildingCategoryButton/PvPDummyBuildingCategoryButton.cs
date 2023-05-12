using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Tutorial.Highlighting;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons
{
    public class PvPDummyBuildingCategoryButton : IPvPBuildingCategoryButton
    {
        public bool IsActiveFeedbackVisible { set { /* empty */ } }
        public PvPBuildingCategory Category => default;

#pragma warning disable 67  // Unused event
        public event EventHandler Clicked;
#pragma warning restore 67  // Unused event

        public PvPHighlightArgs CreateHighlightArgs(IPvPHighlightArgsFactory highlightArgsFactory)
        {
            return null;
        }

        public void TriggerClick() { }
    }
}