using BattleCruisers.Tutorial.Highlighting.Masked;
using System;
using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    public enum PositionChangeSource
    {
        NavigationWhell,
        Other
    }

    public class PositionChangedEventArgs : EventArgs
    {
        public PositionChangeSource Source { get; }

        public PositionChangedEventArgs(PositionChangeSource source)
        {
            this.Source = source;
        }
    }

    public interface INavigationWheel : IMaskHighlightable
    {
        Vector2 CenterPosition { get; }

        event EventHandler<PositionChangedEventArgs> CenterPositionChanged;

        void SetCenterPosition(Vector2 centerPosition);
    }
}