using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleCruisers.Data;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.BattleScene.Heckles;
using BattleCruisers.Utils.Localisation;

namespace BattleCruisers.Scenes.BattleScene
{
    public class ExtendedNPCHeckleManager : MonoBehaviour
    {
        private HeckleMessage heckleMessage;
        private List<Coroutine> activeDialogCoroutines = new List<Coroutine>();

        public void Initialize(HeckleMessage message, List<ChainBattleChat> chats)
        {
            heckleMessage = message;
            // Dialogs are now handled through localization system
        }

        public void ShowChainBattleChat(string chatKey, ChainBattleChat.SpeakerType speaker = ChainBattleChat.SpeakerType.EnemyCaptain, float duration = 4f)
        {
            StartCoroutine(ShowChatCoroutine(chatKey, speaker, duration));
        }

        private IEnumerator ShowChatCoroutine(string chatKey, ChainBattleChat.SpeakerType speaker, float duration)
        {
            // Cancel any existing chat
            foreach (var coroutine in activeDialogCoroutines)
            {
                StopCoroutine(coroutine);
            }
            activeDialogCoroutines.Clear();

            // Get localized text and show with existing heckle system
            string localizedText;
            try
            {
                localizedText = LocTableCache.StoryTable.GetString(chatKey);
                // If the text is the same as the key (meaning not found), provide demo fallback
                if (localizedText == chatKey && chatKey.StartsWith("level"))
                {
                    localizedText = GetDemoFallbackText(chatKey);
                }
            }
            catch
            {
                // Fallback for demo
                localizedText = GetDemoFallbackText(chatKey);
            }

            string styledText = GetStyledText(localizedText, speaker);

            // Use existing heckle system - this would need to be integrated properly
            // For now, use a simplified approach
            Debug.Log($"ChainBattle Dialog: {styledText}");

            yield return new WaitForSeconds(duration);
        }

        private string GetStyledText(string text, ChainBattleChat.SpeakerType speaker)
        {
            string prefix = "";
            switch (speaker)
            {
                case ChainBattleChat.SpeakerType.EnemyCaptain:
                    prefix = "[ENEMY] ";
                    break;
                case ChainBattleChat.SpeakerType.PlayerCaptain:
                    prefix = "[ALLY] ";
                    break;
                case ChainBattleChat.SpeakerType.Narrative:
                    prefix = "[NARRATIVE] ";
                    break;
            }
            return prefix + text;
        }

        string GetDemoFallbackText(string chatKey)
        {
            // Demo ChainBattle fallback texts for level999
            switch (chatKey)
            {
                case "level999/EnemyIntro": return "Not you again! How are you still alive?";
                case "level999/PlayerResponse": return "Hey there! Ready for round two?";
                case "level999/EnemyTaunt": return "That's just my escort ship!";
                case "level999/PlayerSurprise": return "What is this?! A second cruiser?!";
                case "level999/EnemyFinalWarning": return "This is your final warning!";
                case "level999/EnemyCritical": return "I can't believe you don't secure your wifi!";
                case "level999/EnemyReactionAirFactory": return "An Air Factory? Let me counter with superior air defenses!";
                case "level999/DroneAppraisal": return "Excellent work, Captain! The ChainBattle victory has unlocked new technologies.";
                default: return chatKey; // Return key if no fallback found
            }
        }
    }
}
