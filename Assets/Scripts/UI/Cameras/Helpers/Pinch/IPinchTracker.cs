using System;
using UnityEngine;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.Cameras.Helpers.Pinch
{
    public class PinchEventArgs : EventArgs
    {
        public Vector2 Position { get; }
        public float DeltaInM { get; }

        public PinchEventArgs(Vector2 position, float deltaInM)
        {
            Position = position;
            DeltaInM = deltaInM;
        }

        public override bool Equals(object obj)
        {
            return
                obj is PinchEventArgs other
                && Position.SmartEquals(other.Position)
                && DeltaInM == other.DeltaInM;
        }

        public override int GetHashCode()
        {
            return this.GetHashCode(Position, DeltaInM);
        }
    }

    /// <summary>
    /// To track when the user pinches to zoom in or out.
    /// </summary>
    public interface IPinchTracker
    {
        event EventHandler PinchStart;
        event EventHandler<PinchEventArgs> Pinch;
        event EventHandler PinchEnd;
    }
}