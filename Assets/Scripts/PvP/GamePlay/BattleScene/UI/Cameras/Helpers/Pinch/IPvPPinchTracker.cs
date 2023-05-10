using System;
using UnityEngine;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Helpers.Pinch
{
    public class PvPPinchEventArgs : EventArgs
    {
        public Vector2 Position { get; }
        public float DeltaInM { get; }

        public PvPPinchEventArgs(Vector2 position, float deltaInM)
        {
            Position = position;
            DeltaInM = deltaInM;
        }

        public override bool Equals(object obj)
        {
            return
                obj is PvPPinchEventArgs other
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
    public interface IPvPPinchTracker
    {
        event EventHandler PinchStart;
        event EventHandler<PvPPinchEventArgs> Pinch;
        event EventHandler PinchEnd;
    }
}