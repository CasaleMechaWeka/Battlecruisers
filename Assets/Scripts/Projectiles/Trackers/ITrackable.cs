using System;
using UnityEngine;

namespace BattleCruisers.Projectiles.Trackers
{
    public interface ITrackable
    {
        Vector3 Position { get; }

        event EventHandler PositionChanged;
        event EventHandler Destroyed;
    }
}