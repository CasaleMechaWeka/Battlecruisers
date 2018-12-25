using BattleCruisers.Utils.BattleScene;
using System;
using UnityEngine;

namespace BattleCruisers.Projectiles
{
    public interface IProjectile : IDestructable
    {
        Vector2 Position { get; }

        event EventHandler Destroyed;
    }
}