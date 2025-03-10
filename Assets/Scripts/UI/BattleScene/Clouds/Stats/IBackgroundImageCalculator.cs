using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Clouds.Stats
{
    public interface IBackgroundImageCalculator
    {
        Vector3 FindPosition(IBackgroundImageStats stats, float cameraAspectRatio);
    }
}