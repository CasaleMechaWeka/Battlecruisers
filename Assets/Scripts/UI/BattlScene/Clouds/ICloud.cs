using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Clouds
{
    public interface ICloud
    {
        Vector2 Size { get; }

        void Initialise(float horizontalMovementSpeedInMPerS, float disappearLineInM, float reappearLineInM);
    }
}
