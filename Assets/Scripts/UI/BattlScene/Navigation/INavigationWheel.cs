using System;
using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    public interface INavigationWheel
    {
        Vector2 CenterPosition { get; }

        event EventHandler CenterPositionChanged;
    }
}