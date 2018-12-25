using System;
using UnityEngine;

namespace BattleCruisers.Projectiles.Trackers
{
    public interface ITrackable
    {
        Vector2 Position { get; }

        event EventHandler PositionChanged;
        event EventHandler Destroyed;
    }
}