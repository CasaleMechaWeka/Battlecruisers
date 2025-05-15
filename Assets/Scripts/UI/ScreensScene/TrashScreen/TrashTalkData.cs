using BattleCruisers.Utils.Localisation;
using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.TrashScreen
{
    public class TrashTalkData : Prefab
    {
        public Sprite enemySprite;
        public Sprite EnemySprite => enemySprite;
        public GameObject enemyPrefab;
        public GameObject EnemyPrefab => enemyPrefab;

        public string enemyName = "Bill";
        public string EnemyName => enemyName;

        public float enemyScale = 1;
        public float EnemyScale => enemyScale;

        public bool playerTalksFirst;
        public bool PlayerTalksFirst => playerTalksFirst;

        public string PlayerText { get; private set; }
        public string EnemyText { get; private set; }
        public string AppraisalDroneText { get; private set; }

        public string stringKeyBase;

        public string StringKeyBase => stringKeyBase;

        public void Initialise(bool isSideQuest = false)
        {

            string playerTextKey = $"{stringKeyBase}/PlayerText";
            string enemyTextKey = $"{stringKeyBase}/EnemyText";
            string droneTextKey = $"{stringKeyBase}/DroneText";
            string enemyNameKey = $"{stringKeyBase}/name";

            PlayerText = LocTableCache.StoryTable.GetString(playerTextKey);
            EnemyText = LocTableCache.StoryTable.GetString(enemyTextKey);
            AppraisalDroneText = LocTableCache.StoryTable.GetString(droneTextKey);
            enemyName = LocTableCache.StoryTable.GetString(enemyNameKey);
        }
    }
}