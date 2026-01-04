using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace BattleCruisers.Utils.Debugging
{
    /// <summary>
    /// Displays admin/debug messages on screen during battle.
    /// Attach this to a GameObject with a Text component in your admin panel canvas.
    /// </summary>
    public class BattleSceneMessageDisplay : MonoBehaviour
    {
        [Header("Message Display Settings")]
        [Tooltip("Text component to display messages. Drag from your admin panel canvas.")]
        public Text messageText;

        [Tooltip("Maximum number of messages to show at once")]
        public int maxMessages = 5;

        [Tooltip("How long each message stays visible (seconds)")]
        public float messageLifetime = 8f;

        [Tooltip("Auto-scroll to show newest messages")]
        public bool autoScroll = true;

        private Queue<MessageEntry> messageQueue = new Queue<MessageEntry>();
        private float lastUpdateTime = 0f;

        private class MessageEntry
        {
            public string text;
            public float timestamp;
            public Color color;

            public MessageEntry(string text, Color color)
            {
                this.text = text;
                this.timestamp = Time.time;
                this.color = color;
            }
        }

        void Start()
        {
            if (messageText == null)
            {
                messageText = GetComponent<Text>();
            }

            if (messageText == null)
            {
                Debug.LogWarning("[BattleSceneMessageDisplay] No Text component found. Messages will only appear in console.");
            }

            // Note: Visibility is controlled by CheaterButtonsPanelToggler
            // Messages will queue even when inactive and display when panel opens
        }

        void Update()
        {
            // Only update if GameObject is active
            if (!gameObject.activeSelf)
                return;

            // Clean up old messages
            while (messageQueue.Count > 0 && Time.time - messageQueue.Peek().timestamp > messageLifetime)
            {
                messageQueue.Dequeue();
            }

            // Update display
            if (messageText != null && messageQueue.Count > 0)
            {
                UpdateDisplay();
            }
        }

        /// <summary>
        /// Show a message on screen and in console
        /// Messages are queued even when inactive, so they appear when the panel is shown
        /// </summary>
        public void ShowMessage(string message, MessageType type = MessageType.Info)
        {
            Color color = GetColorForType(type);
            string prefix = GetPrefixForType(type);
            string fullMessage = $"{prefix}{message}";

            // Log to console (always, even if display is inactive)
            switch (type)
            {
                case MessageType.Error:
                    Debug.LogError($"[BattleSequencer] {message}");
                    break;
                case MessageType.Warning:
                    Debug.LogWarning($"[BattleSequencer] {message}");
                    break;
                default:
                    Debug.Log($"[BattleSequencer] {message}");
                    break;
            }

            // Add to display queue (even when inactive - messages will show when panel opens)
            messageQueue.Enqueue(new MessageEntry(fullMessage, color));
            
            // Limit queue size
            while (messageQueue.Count > maxMessages)
            {
                messageQueue.Dequeue();
            }

            // Only update display if active
            if (gameObject.activeSelf && messageText != null)
            {
                UpdateDisplay();
            }
        }

        /// <summary>
        /// Update the display text. Can be called externally to refresh when panel becomes active.
        /// </summary>
        public void UpdateDisplay()
        {
            if (messageText == null || messageQueue.Count == 0)
            {
                if (messageText != null)
                    messageText.text = "";
                return;
            }

            // Build display text
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            MessageEntry[] messages = messageQueue.ToArray();

            // Show newest messages first if auto-scroll
            int startIndex = autoScroll ? messages.Length - 1 : 0;
            int endIndex = autoScroll ? -1 : messages.Length;
            int step = autoScroll ? -1 : 1;

            for (int i = startIndex; autoScroll ? i >= endIndex : i < endIndex; i += step)
            {
                MessageEntry msg = messages[i];
                float age = Time.time - msg.timestamp;
                float alpha = Mathf.Clamp01(1f - (age / messageLifetime));
                
                // Apply color with fading alpha
                Color displayColor = msg.color;
                displayColor.a = alpha;
                
                string colorHex = ColorUtility.ToHtmlStringRGBA(displayColor);
                sb.AppendLine($"<color=#{colorHex}>{msg.text}</color>");
            }

            messageText.text = sb.ToString();
        }

        private Color GetColorForType(MessageType type)
        {
            switch (type)
            {
                case MessageType.Error:
                    return Color.red;
                case MessageType.Warning:
                    return Color.yellow;
                case MessageType.Success:
                    return Color.green;
                case MessageType.Boost:
                    return Color.cyan;
                case MessageType.Building:
                    return Color.magenta;
                default:
                    return Color.white;
            }
        }

        private string GetPrefixForType(MessageType type)
        {
            switch (type)
            {
                case MessageType.Error:
                    return "[ERROR] ";
                case MessageType.Warning:
                    return "[WARN] ";
                case MessageType.Success:
                    return "[OK] ";
                case MessageType.Boost:
                    return "[BOOST] ";
                case MessageType.Building:
                    return "[BUILD] ";
                default:
                    return "";
            }
        }

        public enum MessageType
        {
            Info,
            Success,
            Warning,
            Error,
            Building,
            Boost
        }
    }
}

