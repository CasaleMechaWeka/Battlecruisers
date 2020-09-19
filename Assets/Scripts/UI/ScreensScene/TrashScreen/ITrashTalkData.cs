using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.TrashScreen
{
    public interface ITrashTalkData
    {
        Sprite EnemyImage { get; }
        string EnemyName { get; }
        bool PlayerTalksFirst { get; }
        string PlayerText { get; }
        string EnemyText { get; }
    }
}