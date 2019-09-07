using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Effects
{
    public interface IDroneController : IPoolable<Vector2>
    {
        void Deactivate();
    }
}