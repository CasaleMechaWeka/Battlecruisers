using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using BattleCruisers.Data;
using BattleCruisers.Utils.Localisation;

namespace BattleCruisers.UI.BattleScene.Heckles
{
    public class ChainBattleHeckleMessage : HeckleMessage
    {
        public new void Show(int heckleIndex)
        {
            // Use 'new' instead of 'override' since base method is not virtual
            base.Show(heckleIndex);
        }

        public void ShowCustomChat(ChainBattleChat chat)
        {
            // Custom display logic for ChainBattle chats
            string styledText = GetStyledChatText(chat);

            // Display using existing message component
            message.text = styledText;

            // Use reflection or create a custom animation since messageFrame is private
            var messageFrame = GetComponent<RectTransform>();
            if (messageFrame != null)
            {
                messageFrame.localScale = Vector3.zero;
                messageFrame.DOScale(Vector3.one * 1.5f, 0.2f);
            }

            CancelInvoke("Hide");
            Invoke("Hide", chat.displayDuration);
        }

        private string GetStyledChatText(ChainBattleChat chat)
        {
            string prefix = "";
            Color textColor = Color.white;

            switch (chat.speaker)
            {
                case ChainBattleChat.SpeakerType.EnemyCaptain:
                    prefix = "[ENEMY] ";
                    textColor = Color.red;
                    break;
                case ChainBattleChat.SpeakerType.PlayerCaptain:
                    prefix = "[ALLY] ";
                    textColor = Color.blue;
                    break;
                case ChainBattleChat.SpeakerType.Narrative:
                    prefix = "[NARRATIVE] ";
                    textColor = Color.gray;
                    break;
            }

            message.color = textColor;
            return prefix + BattleCruisers.Utils.Localisation.LocTableCache.StoryTable.GetString(chat.chatKey);
        }
    }
}
