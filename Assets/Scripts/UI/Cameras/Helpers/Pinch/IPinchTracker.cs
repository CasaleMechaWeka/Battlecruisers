using System;
using UnityEngine;

// FELIX  Remove?
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