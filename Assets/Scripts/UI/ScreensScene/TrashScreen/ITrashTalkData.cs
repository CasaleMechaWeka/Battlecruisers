using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.TrashScreen
{
    public interface ITrashTalkData
    {
        Sprite EnemySprite { get; }
        GameObject EnemyPrefab { get; }
        string EnemyName { get; }
        float EnemyScale { get; }
        bool PlayerTalksFirst { get; }
        string PlayerText { get; }
        string EnemyText { get; }
        string AppraisalDroneText { get; }
        string StringKeyBase { get; }
    }
}