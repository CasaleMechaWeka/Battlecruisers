using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.UI.BattleScene.ProgressBars
{
    public interface IHealthBar : IGameObject
    {
        Vector2 Offset { get; set; }
        bool FollowDamagable { get; set; }
    }
}