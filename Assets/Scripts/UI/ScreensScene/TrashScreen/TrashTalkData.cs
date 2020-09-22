using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.TrashScreen
{
    public class TrashTalkData : MonoBehaviour, ITrashTalkData
    {
        public Sprite enemyImage;
        public Sprite EnemyImage => enemyImage;

        public string enemyName = "Bill";
        public string EnemyName => enemyName;

        public float enemyScale = 1;
        public float EnemyScale => enemyScale;

        public Vector2 enemyPosition = new Vector2(650, 450);
        public Vector2 EnemyPosition => enemyPosition;

        public bool playerTalksFirst;
        public bool PlayerTalksFirst => playerTalksFirst;

        public string playerText = "Sup";
        public string PlayerText => playerText;

        public string enemyText = "Kia Ora";
        public string EnemyText => enemyText;
    }
}