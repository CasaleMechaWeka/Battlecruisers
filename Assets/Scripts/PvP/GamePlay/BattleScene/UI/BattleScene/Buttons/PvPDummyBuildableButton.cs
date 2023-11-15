using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Tutorial.Highlighting;
using System;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons
{
    public class PvPDummyBuildableButton : IPvPBuildableButton
    {
        public IPvPBuildable Buildable => null;
        public Color Color { set { /* empty */ } }
        public bool IsPresented { get; set; }

#pragma warning disable 67  // Unused event
        public event EventHandler Clicked;
        public event EventHandler Dismissed;
#pragma warning restore 67  // Unused event

        public PvPHighlightArgs CreateHighlightArgs(IPvPHighlightArgsFactory highlightArgsFactory)
        {
            return null;
        }

        public void OnDismissing() { }
        public void OnPresenting(object activationParameter) { }
        public void TriggerClick() { }
    }
}