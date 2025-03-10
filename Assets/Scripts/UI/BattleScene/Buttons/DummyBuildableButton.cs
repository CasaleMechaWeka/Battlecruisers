using BattleCruisers.Buildables;
using BattleCruisers.Tutorial.Highlighting;
using System;
using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public class DummyBuildableButton : IBuildableButton
    {
        public IBuildable Buildable => null;
        public Color Color { set { /* empty */ } }
        public bool IsPresented { get; set; }

#pragma warning disable 67  // Unused event
        public event EventHandler Clicked;
        public event EventHandler Dismissed;
#pragma warning restore 67  // Unused event

        public HighlightArgs CreateHighlightArgs(IHighlightArgsFactory highlightArgsFactory)
        {
            return null;
        }

        public void OnDismissing() { }
        public void OnPresenting(object activationParameter) { }
        public void TriggerClick() { }
    }
}