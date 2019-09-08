using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.UI.BattleScene.ProgressBars
{
    public interface IHealthBar : IGameObject
    {
        Vector3 Offset { get; set; }
    }
}