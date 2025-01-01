using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Tutorial.Highlighting;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons
{
    public class PvPDummyBuildingCategoryButton : IPvPBuildingCategoryButton
    {
        public bool IsActiveFeedbackVisible { set { /* empty */ } }
        public BuildingCategory Category => default;

#pragma warning disable 67  // Unused event
        public event EventHandler Clicked;
#pragma warning restore 67  // Unused event

        public HighlightArgs CreateHighlightArgs(IHighlightArgsFactory highlightArgsFactory)
        {
            return null;
        }

        public void TriggerClick() { }
    }
}