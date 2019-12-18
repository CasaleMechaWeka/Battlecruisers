using BattleCruisers.Tutorial.Highlighting.Masked;
using System;
using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    public class PositionChangedEventArgs : EventArgs
    {
        public bool SnapToCorners { get; }

        public PositionChangedEventArgs(bool snapToCorners)
        {
            SnapToCorners = snapToCorners;
        }
    }

    public interface INavigationWheel : IMaskHighlightable
    {
        Vector2 CenterPosition { get; }

        event EventHandler<PositionChangedEventArgs> CenterPositionChanged;

        /// <param name="snapToCorners">
        /// True if positions in the corners of the navigation wheel pyramid should snap
        /// to those corner positions.  False if the navigation wheel should respect
        /// the given centerPosition without adjustments.
        /// </param>
        void SetCenterPosition(Vector2 centerPosition, bool snapToCorners);
    }
}