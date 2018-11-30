using BattleCruisers.Tutorial.Highlighting.Masked;
using System;
using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    public interface INavigationWheel : IMaskHighlightable
    {
        Vector2 CenterPosition { get; set; }

        event EventHandler CenterPositionChanged;
    }
}