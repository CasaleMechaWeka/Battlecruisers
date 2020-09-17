using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.TrashScreen
{
    public interface ITrashTalkData
    {
        // FELIX  Avoid duplication (enemy image, enemy name) with levels screen
        Sprite EnemyImage { get; }
        string EnemyName { get; }
        bool PlayerTalksFirst { get; }
        string PlayerText { get; }
        string EnemyText { get; }
    }
}