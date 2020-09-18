using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.TrashScreen
{
    public class TrashTalkBubblesController : MonoBehaviour
    {
        public BubbleController leftTop, rightBottom;
        public BubbleController rightTop, leftBottom;

        public const string PLAYER_NAME = "Charlie";

        public void Initialise(ITrashTalkData trashTalkData)
        {
            Helper.AssertIsNotNull(leftTop, rightBottom, rightTop, leftBottom);
            Assert.IsNotNull(trashTalkData);

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
            
            playerBubble.Initialise(PLAYER_NAME, trashTalkData.PlayerText);
            playerBubble.gameObject.SetActive(true);

            enemyBubble.Initialise(trashTalkData.EnemyName, trashTalkData.EnemyText);
            enemyBubble.gameObject.SetActive(true);
        }
    }
}