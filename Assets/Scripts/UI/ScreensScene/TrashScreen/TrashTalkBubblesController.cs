using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.TrashScreen
{
    public class TrashTalkBubblesController : MonoBehaviour
    {
        public BubbleController leftTop, rightBottom;
        public BubbleController rightTop, leftBottom;

        public void Initialise(ITrashTalkData trashTalkData)
        {
            Helper.AssertIsNotNull(leftTop, rightBottom, rightTop, leftBottom);
            Helper.AssertIsNotNull(trashTalkData);

            BubbleController playerBubble, enemyBubble;

            if (trashTalkData.PlayerTalksFirst)
            {
                playerBubble = leftTop;
                enemyBubble = rightBottom;
                leftBottom.gameObject.SetActive(false);
                rightTop.gameObject.SetActive(false);
            }
            else
            {
                playerBubble = leftBottom;
                enemyBubble = rightTop;
                leftTop.gameObject.SetActive(false);
                rightBottom.gameObject.SetActive(false);
            }

            string protagonistName = LocTableCache.CommonTable.GetString("Names/Protagonist");

            playerBubble.Initialise(protagonistName, trashTalkData.PlayerText);
            playerBubble.gameObject.SetActive(true);

            string enemyNameKey = $"{trashTalkData.StringKeyBase}/name";
            Debug.Log($"EnemyName key: {enemyNameKey}");
            string enemyName = LocTableCache.StoryTable.GetString(enemyNameKey);
            Debug.Log($"Resolved EnemyName: {enemyName}");

            enemyBubble.Initialise(enemyName, trashTalkData.EnemyText);
            enemyBubble.gameObject.SetActive(true);
        }

    }
}