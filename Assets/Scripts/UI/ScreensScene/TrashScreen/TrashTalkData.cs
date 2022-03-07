using BattleCruisers.Utils.Localisation;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.TrashScreen
{
    public class TrashTalkData : Prefab, ITrashTalkData
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

        public string PlayerText { get; private set; }
        public string EnemyText { get; private set; }
        public string AppraisalDroneText { get; private set; }

        public string stringKeyBase;

        public void Initialise(ILocTable storyStrings)
        {
            Assert.IsNotNull(storyStrings);
            //Debug.Log(stringKeyBase);
            PlayerText = storyStrings.GetString($"{stringKeyBase}/PlayerText");
            EnemyText = storyStrings.GetString($"{stringKeyBase}/EnemyText");
            AppraisalDroneText = storyStrings.GetString($"{stringKeyBase}/DroneText");
        }
    }
}