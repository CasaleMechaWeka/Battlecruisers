using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.Ads
{
    /// <summary>
    /// Creates a kill switch UI overlay that appears above stuck ads.
    /// Attach this to the AppLovinManager GameObject and it will auto-create the UI.
    /// </summary>
    public class AdKillSwitchUI : MonoBehaviour
    {
        [Header("Auto-Setup")]
        [Tooltip("Automatically create the UI on Start if not assigned")]
        [SerializeField] private bool autoCreateUI = true;
        
        [Header("UI References (auto-assigned if autoCreateUI is true)")]
        public Canvas killSwitchCanvas;
        public Button killSwitchButton;
        public Text killSwitchTimerText;

        private void Start()
        {
            if (autoCreateUI && killSwitchCanvas == null)
            {
                CreateKillSwitchUI();
            }
            
            // Pass references to AppLovinManager
            var appLovinManager = GetComponent<AppLovinManager>();
            if (appLovinManager != null)
            {
                // Use reflection to set the private fields
                var type = appLovinManager.GetType();
                type.GetField("killSwitchCanvas", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    ?.SetValue(appLovinManager, killSwitchCanvas);
                type.GetField("killSwitchButton", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    ?.SetValue(appLovinManager, killSwitchButton);
                type.GetField("killSwitchTimerText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    ?.SetValue(appLovinManager, killSwitchTimerText);
                    
                Debug.Log("[AdKillSwitchUI] Kill switch UI initialized and linked to AppLovinManager");
            }
        }

        private void CreateKillSwitchUI()
        {
            // Create Canvas
            GameObject canvasObj = new GameObject("AdKillSwitchCanvas");
            canvasObj.transform.SetParent(transform, false);
            
            killSwitchCanvas = canvasObj.AddComponent<Canvas>();
            killSwitchCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            killSwitchCanvas.sortingOrder = 32767; // Maximum sort order to appear above everything
            
            CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;
            
            canvasObj.AddComponent<GraphicRaycaster>();

            // Create semi-transparent background
            GameObject bgObj = new GameObject("Background");
            bgObj.transform.SetParent(canvasObj.transform, false);
            RectTransform bgRect = bgObj.AddComponent<RectTransform>();
            bgRect.anchorMin = Vector2.zero;
            bgRect.anchorMax = Vector2.one;
            bgRect.sizeDelta = Vector2.zero;
            
            Image bgImage = bgObj.AddComponent<Image>();
            bgImage.color = new Color(0, 0, 0, 0.7f); // Semi-transparent black

            // Create button container (centered)
            GameObject buttonContainer = new GameObject("ButtonContainer");
            buttonContainer.transform.SetParent(canvasObj.transform, false);
            RectTransform containerRect = buttonContainer.AddComponent<RectTransform>();
            containerRect.anchorMin = new Vector2(0.5f, 0.5f);
            containerRect.anchorMax = new Vector2(0.5f, 0.5f);
            containerRect.pivot = new Vector2(0.5f, 0.5f);
            containerRect.sizeDelta = new Vector2(600, 250);

            // Create background panel for button
            GameObject panelObj = new GameObject("Panel");
            panelObj.transform.SetParent(buttonContainer.transform, false);
            RectTransform panelRect = panelObj.AddComponent<RectTransform>();
            panelRect.anchorMin = Vector2.zero;
            panelRect.anchorMax = Vector2.one;
            panelRect.sizeDelta = Vector2.zero;
            
            Image panelImage = panelObj.AddComponent<Image>();
            panelImage.color = new Color(0.2f, 0.2f, 0.2f, 0.95f); // Dark gray background

            // Create timer text (above button)
            GameObject timerTextObj = new GameObject("TimerText");
            timerTextObj.transform.SetParent(buttonContainer.transform, false);
            RectTransform timerRect = timerTextObj.AddComponent<RectTransform>();
            timerRect.anchorMin = new Vector2(0.5f, 0.65f);
            timerRect.anchorMax = new Vector2(0.5f, 0.65f);
            timerRect.pivot = new Vector2(0.5f, 0.5f);
            timerRect.sizeDelta = new Vector2(550, 80);
            
            killSwitchTimerText = timerTextObj.AddComponent<Text>();
            killSwitchTimerText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            killSwitchTimerText.fontSize = 32;
            killSwitchTimerText.alignment = TextAnchor.MiddleCenter;
            killSwitchTimerText.color = Color.white;
            killSwitchTimerText.text = "Kill switch in 30s";
            
            // Add outline for better readability
            Outline timerOutline = timerTextObj.AddComponent<Outline>();
            timerOutline.effectColor = Color.black;
            timerOutline.effectDistance = new Vector2(2, -2);

            // Create button
            GameObject buttonObj = new GameObject("KillSwitchButton");
            buttonObj.transform.SetParent(buttonContainer.transform, false);
            RectTransform buttonRect = buttonObj.AddComponent<RectTransform>();
            buttonRect.anchorMin = new Vector2(0.5f, 0.3f);
            buttonRect.anchorMax = new Vector2(0.5f, 0.3f);
            buttonRect.pivot = new Vector2(0.5f, 0.5f);
            buttonRect.sizeDelta = new Vector2(400, 100);
            
            killSwitchButton = buttonObj.AddComponent<Button>();
            Image buttonImage = buttonObj.AddComponent<Image>();
            buttonImage.color = new Color(0.8f, 0.2f, 0.2f, 1f); // Red button
            
            ColorBlock colors = killSwitchButton.colors;
            colors.normalColor = new Color(0.8f, 0.2f, 0.2f, 1f);
            colors.highlightedColor = new Color(1f, 0.3f, 0.3f, 1f);
            colors.pressedColor = new Color(0.6f, 0.1f, 0.1f, 1f);
            killSwitchButton.colors = colors;

            // Create button text
            GameObject buttonTextObj = new GameObject("Text");
            buttonTextObj.transform.SetParent(buttonObj.transform, false);
            RectTransform buttonTextRect = buttonTextObj.AddComponent<RectTransform>();
            buttonTextRect.anchorMin = Vector2.zero;
            buttonTextRect.anchorMax = Vector2.one;
            buttonTextRect.sizeDelta = Vector2.zero;
            
            Text buttonText = buttonTextObj.AddComponent<Text>();
            buttonText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            buttonText.fontSize = 40;
            buttonText.fontStyle = FontStyle.Bold;
            buttonText.alignment = TextAnchor.MiddleCenter;
            buttonText.color = Color.white;
            buttonText.text = "FORCE CLOSE AD";
            
            // Add outline for button text
            Outline buttonOutline = buttonTextObj.AddComponent<Outline>();
            buttonOutline.effectColor = Color.black;
            buttonOutline.effectDistance = new Vector2(2, -2);

            // Initially hide the UI
            canvasObj.SetActive(false);

            Debug.Log("[AdKillSwitchUI] Kill switch UI created successfully");
        }
    }
}

