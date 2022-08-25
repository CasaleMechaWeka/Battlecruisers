using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.TrashScreen
{
    public class TrashTalkBubblesController : MonoBehaviour
    {
        public BubbleController leftTop, rightBottom;
        public BubbleController rightTop, leftBottom;

        public void Initialise(ITrashTalkData trashTalkData, ILocTable commonStrings, ILocTable storyStrings)
        {
            Helper.AssertIsNotNull(leftTop, rightBottom, rightTop, leftBottom);
            Helper.AssertIsNotNull(trashTalkData, commonStrings);

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

            string protagonistName = commonStrings.GetString("Names/Protagonist");

            playerBubble.Initialise(protagonistName, trashTalkData.PlayerText);
            playerBubble.gameObject.SetActive(true);

            string enemyName = storyStrings.GetString($"{trashTalkData.stringKeyBase}/name");

            enemyBubble.Initialise(enemyName, trashTalkData.EnemyText);
            enemyBubble.gameObject.SetActive(true);
        }
    }
}