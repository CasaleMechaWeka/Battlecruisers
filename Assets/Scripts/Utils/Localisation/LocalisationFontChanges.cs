using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using TMPro;
using Utils.Localisation;

namespace BattleCruisers.Utils.Localisation
{
    public class LocalisationFontChanges : MonoBehaviour
    {
        // Debug information shown in inspector
        [Header("Debug Info")]
        [SerializeField] private bool _showDebugInfo = true;
        [SerializeField, ReadOnly] private string _componentType = "None";
        [SerializeField, ReadOnly] private string _originalFont = "None";
        [SerializeField, ReadOnly] private string _currentFont = "None";
        [SerializeField, ReadOnly] private float _currentScaleAdjustment = 1.0f;
        [SerializeField, ReadOnly] private float _currentYAdjustment = 0f;
        [SerializeField, ReadOnly] private bool _isBold = false;
        [SerializeField, ReadOnly] private string _fontStyle = "None";

        private Text _text;
        private TMP_Text _tmpText;
        private Font _newFont;
        private TMP_FontAsset _newTmpFont;
        private int _originalFontMaxSize;
        private int _originalFontMinSize;
        private float _newFontScaleAdjustment;
        private float _yPositionAdjustment;
        private bool _boldNewFont;
        private LocalizeStringEvent _localizeStringEvent;
        private RTLSettings _rtlSettings = new RTLSettings();
        private Vector3 _originalPosition;
        public bool allowScaleAdjustment = true;
        public bool allowPositionAdjustment = true;
        public struct RTLSettings
        {
            public bool IsRtl { get; set; }
            public string RtlCorrectedText { get; set; }
        }

        private void Awake()
        {
            // Store original position
            _originalPosition = transform.localPosition;

            // Try to get Text component
            _text = GetComponent<Text>();

            // If no Text component, try to get TMP_Text component
            if (_text == null)
            {
                _tmpText = GetComponent<TMP_Text>();
            }

            // Update debug info
            if (_showDebugInfo)
            {
                _componentType = _text != null ? "Unity UI Text" : (_tmpText != null ? "TextMeshPro" : "None");
                _originalFont = GetCurrentFontName();
                _currentFont = _originalFont;
                UpdateInspectorInfo();
            }

            if (TryGetComponent(out LocalizeStringEvent localizeStringEvent))
            {
                _localizeStringEvent = localizeStringEvent;
            }
            else if (transform.parent.TryGetComponent(out LocalizeStringEvent localizeStringEventInParent))
            {
                _localizeStringEvent = localizeStringEventInParent;
            }
        }

        private string GetCurrentFontName()
        {
            if (_text != null && _text.font != null)
                return $"{_text.font.name} (UI)";
            if (_tmpText != null && _tmpText.font != null)
                return $"{_tmpText.font.name} (TMP)";
            return "None";
        }

        private void UpdateInspectorInfo()
        {
            if (!_showDebugInfo) return;

            _currentFont = GetCurrentFontName();
            _currentScaleAdjustment = _newFontScaleAdjustment;
            _currentYAdjustment = _yPositionAdjustment;
            _isBold = _boldNewFont;

            if (_showDebugInfo)
            {
                // Debug.Log($"[{gameObject.name}] Component Type: {_componentType}");
                // Debug.Log($"[{gameObject.name}] Original Font: {_originalFont}");
                // Debug.Log($"[{gameObject.name}] Current Font: {_currentFont}");
                // Debug.Log($"[{gameObject.name}] Scale Adjustment: {_currentScaleAdjustment}");
                // Debug.Log($"[{gameObject.name}] Y Position Adjustment: {_currentYAdjustment}");
                // Debug.Log($"[{gameObject.name}] Bold: {_isBold}");
                // Debug.Log($"[{gameObject.name}] Font Style: {_fontStyle}");
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            try
            {
                // Check if we have either a Text or TMP_Text component
                if (_text == null && _tmpText == null)
                {
                    Debug.LogWarning($"{gameObject.name}: {nameof(LocalisationFontChanges)} requires either a {nameof(Text)} or {nameof(TMP_Text)} component.");
                    return;
                }

                // Get original settings if we have a Text component
                if (_text != null)
                {
                    _originalFontMaxSize = _text.resizeTextMaxSize;
                    _originalFontMinSize = _text.resizeTextMinSize;
                }

                _rtlSettings.IsRtl = false;
                _rtlSettings.RtlCorrectedText = "";

                UpdateString();
            }
            catch (Exception e)
            {
                Debug.Log($"font localisation failed - Start: {e.Message}");
            }
        }

        private void OnEnable()
        {
            if (LocalizationSettings.SelectedLocale.LocaleName != "Arabic (ar)") return; // FIX ALL CAPS ISSUE
            if (_localizeStringEvent != null)
            {
                _localizeStringEvent.OnUpdateString.AddListener(OnUpdateString);
                _localizeStringEvent.RefreshString();
            }
            else if (_text != null && _text.text != _rtlSettings.RtlCorrectedText && _rtlSettings.IsRtl == false)
            {
                ConvertToRtl();
            }
            else if (_text != null && _text.text != _rtlSettings.RtlCorrectedText && _rtlSettings.IsRtl)
            {
                ResetRtl();
            }
        }

        private void OnDisable()
        {
            _rtlSettings.IsRtl = false;
            if (_localizeStringEvent != null) _localizeStringEvent.OnUpdateString.RemoveListener(OnUpdateString);
        }

        private void OnUpdateString(string localizedString)
        {
            if (_text != null)
            {
                _text.text = localizedString;
                ConvertToRtl();
            }
            else if (_tmpText != null)
            {
                _tmpText.text = localizedString;
                // TMP doesn't need RTL conversion as it handles it automatically
            }
        }

        private void UpdateString()
        {
            try
            {
                string localeName = LocalizationSettings.SelectedLocale.LocaleName;

                if (_text != null)
                {
                    // Store original sizes before any adjustments
                    int origMinSize = _originalFontMinSize;
                    int origMaxSize = _originalFontMaxSize;

                    // Determine if this is a heading or regular font
                    bool isHeading = _text.font.name.Contains("MechaWeka", StringComparison.OrdinalIgnoreCase);
                    _fontStyle = isHeading ? "Heading" : "Regular";

                    // Load the appropriate font
                    Font localeSpecificFont = GetLocaleSpecificFont(localeName, _text.font.name);

                    if (localeSpecificFont != null)
                    {
                        _text.font = localeSpecificFont;
                    }

                    // First try the original key that we know works
                    string bestFitScaleAdjustment = LocTableCache.FontsTable.GetString("BestFitScaleAdjustment");
                    // Debug.Log($"[{gameObject.name}] Retrieved original scale adjustment 'BestFitScaleAdjustment': '{bestFitScaleAdjustment}'");

                    // Check if the original key returns an error or is empty
                    if (string.IsNullOrEmpty(bestFitScaleAdjustment) ||
                        bestFitScaleAdjustment.Contains("Not localised") ||
                        bestFitScaleAdjustment.Contains("is Not localised"))
                    {
                        // Only try the new specific keys if the original key doesn't work
                        string scaleAdjKey = isHeading ? "HeadingScaleAdjustment" : "RegularScaleAdjustment";
                        bestFitScaleAdjustment = LocTableCache.FontsTable.GetString(scaleAdjKey);
                        // Debug.Log($"[{gameObject.name}] Falling back to specific scale adjustment '{scaleAdjKey}': '{bestFitScaleAdjustment}'");
                    }

                    // Check if we got an error message
                    if (!string.IsNullOrEmpty(bestFitScaleAdjustment) &&
                        (bestFitScaleAdjustment.Contains("Not localised") || bestFitScaleAdjustment.Contains("is Not localised")))
                    {
                        Debug.LogWarning($"[{gameObject.name}] The localization key returned an error. Using default 1.0.");
                        bestFitScaleAdjustment = "1.0"; // Use a safe default
                    }

                    if (!string.IsNullOrEmpty(bestFitScaleAdjustment))
                    {
                        if (float.TryParse(bestFitScaleAdjustment, NumberStyles.Float, CultureInfo.InvariantCulture, out float scale))
                        {
                            _newFontScaleAdjustment = scale;
                            // Debug.Log($"[{gameObject.name}] Successfully parsed scale adjustment: {_newFontScaleAdjustment} (Using {(usingLegacyKey ? "legacy" : "specific")} key)");
                        }
                        else
                        {
                            Debug.LogError($"[{gameObject.name}] Failed to parse scale adjustment value: '{bestFitScaleAdjustment}'");
                            _newFontScaleAdjustment = 1.0f;
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"[{gameObject.name}] No scale adjustment found in the string table. Using default 1.0");
                        _newFontScaleAdjustment = 1.0f;
                    }

                    // Similar approach for Y adjustment
                    string yAdjustment = "0"; // Default to 0
                    if (allowPositionAdjustment)
                    {
                        string yAdjustKey = isHeading ? "HeadingAdjustY" : "RegularAdjustY";
                        yAdjustment = LocTableCache.FontsTable.GetString(yAdjustKey);
                        // Debug.Log($"[{gameObject.name}] Retrieved Y adjustment '{yAdjustKey}': '{yAdjustment}'");

                        // Check if the value is actually an error message
                        if (!string.IsNullOrEmpty(yAdjustment) &&
                            (yAdjustment.Contains("Not localised") || yAdjustment.Contains("is Not localised")))
                        {
                            Debug.LogWarning($"[{gameObject.name}] The key '{yAdjustKey}' is not localized yet. Using default 0.");
                            yAdjustment = "0"; // Use a safe default
                        }
                    }

                    if (!string.IsNullOrEmpty(yAdjustment))
                    {
                        if (float.TryParse(yAdjustment, NumberStyles.Float, CultureInfo.InvariantCulture, out float yAdj))
                        {
                            _yPositionAdjustment = yAdj;
                        }
                        else
                        {
                            Debug.LogError($"[{gameObject.name}] Failed to parse Y adjustment value: '{yAdjustment}'");
                            _yPositionAdjustment = 0f;
                        }
                    }
                    else
                    {
                        _yPositionAdjustment = 0f;
                    }

                    // Apply bold if needed
                    string boldNewFontBool = LocTableCache.FontsTable.GetString("Bold");
                    Boolean.TryParse(boldNewFontBool, out _boldNewFont);
                    if (_boldNewFont)
                    {
                        _text.fontStyle = FontStyle.Bold;
                    }

                    // Apply scale adjustment
                    if (allowScaleAdjustment)
                    {
                        int newMinSize = Mathf.RoundToInt(origMinSize * _newFontScaleAdjustment);
                        int newMaxSize = Mathf.RoundToInt(origMaxSize * _newFontScaleAdjustment);

                        //Debug.Log($"[{gameObject.name}] Scaling font: Original min={origMinSize}, max={origMaxSize}, " +
                        //          $"Scale={_newFontScaleAdjustment}, New min={newMinSize}, max={newMaxSize}");

                        _text.resizeTextMinSize = newMinSize;
                        _text.resizeTextMaxSize = newMaxSize;
                    }

                    // Apply Y position adjustment
                    if (allowPositionAdjustment && _yPositionAdjustment != 0)
                    {
                        Vector3 newPosition = _originalPosition;
                        newPosition.y += _yPositionAdjustment;
                        transform.localPosition = newPosition;
                        //Debug.Log($"[{gameObject.name}] Adjusting Y position: Original Y={_originalPosition.y}, " +
                        //          $"Adjustment={_yPositionAdjustment}, New Y={newPosition.y}");
                    }
                    else if (allowPositionAdjustment)
                    {
                        // Reset to original position
                        transform.localPosition = _originalPosition;
                    }
                }
                else if (_tmpText != null)
                {
                    // Store original sizes before any adjustments
                    float origMinSize = _tmpText.fontSizeMin;
                    float origMaxSize = _tmpText.fontSizeMax;

                    // Determine if this is a heading or regular font
                    bool isHeading = _tmpText.font.name.Contains("MechaWeka", StringComparison.OrdinalIgnoreCase);
                    _fontStyle = isHeading ? "Heading" : "Regular";

                    // Load the appropriate font
                    TMP_FontAsset localeSpecificTmpFont = GetLocaleSpecificTmpFont(localeName);

                    if (localeSpecificTmpFont != null)
                    {
                        _tmpText.font = localeSpecificTmpFont;
                    }

                    // First try the original key that we know works
                    string bestFitScaleAdjustment = LocTableCache.FontsTable.GetString("BestFitScaleAdjustment");
                    // Debug.Log($"[{gameObject.name}] Retrieved original TMP scale adjustment 'BestFitScaleAdjustment': '{bestFitScaleAdjustment}'");

                    // Check if the original key returns an error or is empty
                    if (string.IsNullOrEmpty(bestFitScaleAdjustment) ||
                        bestFitScaleAdjustment.Contains("Not localised") ||
                        bestFitScaleAdjustment.Contains("is Not localised"))
                    {
                        // Only try the new specific keys if the original key doesn't work
                        string scaleAdjKey = isHeading ? "HeadingScaleAdjustment" : "RegularScaleAdjustment";
                        bestFitScaleAdjustment = LocTableCache.FontsTable.GetString(scaleAdjKey);
                        Debug.Log($"[{gameObject.name}] Falling back to specific TMP scale adjustment '{scaleAdjKey}': '{bestFitScaleAdjustment}'");
                    }

                    // Check if we got an error message
                    if (!string.IsNullOrEmpty(bestFitScaleAdjustment) &&
                        (bestFitScaleAdjustment.Contains("Not localised") || bestFitScaleAdjustment.Contains("is Not localised")))
                    {
                        Debug.LogWarning($"[{gameObject.name}] The TMP localization key returned an error. Using default 1.0.");
                        bestFitScaleAdjustment = "1.0"; // Use a safe default
                    }

                    if (!string.IsNullOrEmpty(bestFitScaleAdjustment))
                    {
                        if (float.TryParse(bestFitScaleAdjustment, NumberStyles.Float, CultureInfo.InvariantCulture, out float scale))
                        {
                            _newFontScaleAdjustment = scale;
                            //Debug.Log($"[{gameObject.name}] Successfully parsed TMP scale adjustment: {_newFontScaleAdjustment} (Using {(usingLegacyKey ? "legacy" : "specific")} key)");
                        }
                        else
                        {
                            Debug.LogError($"[{gameObject.name}] Failed to parse TMP scale adjustment value: '{bestFitScaleAdjustment}'");
                            _newFontScaleAdjustment = 1.0f;
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"[{gameObject.name}] No TMP scale adjustment found in the string table. Using default 1.0");
                        _newFontScaleAdjustment = 1.0f;
                    }

                    // Similar approach for Y adjustment
                    string yAdjustment = "0"; // Default to 0
                    if (allowPositionAdjustment)
                    {
                        string yAdjustKey = isHeading ? "HeadingAdjustY" : "RegularAdjustY";
                        yAdjustment = LocTableCache.FontsTable.GetString(yAdjustKey);
                        // Debug.Log($"[{gameObject.name}] Retrieved TMP Y adjustment '{yAdjustKey}': '{yAdjustment}'");

                        // Check if the value is actually an error message
                        if (!string.IsNullOrEmpty(yAdjustment) &&
                            (yAdjustment.Contains("Not localised") || yAdjustment.Contains("is Not localised")))
                        {
                            Debug.LogWarning($"[{gameObject.name}] The key '{yAdjustKey}' is not localized yet for TMP. Using default 0.");
                            yAdjustment = "0"; // Use a safe default
                        }
                    }

                    if (!string.IsNullOrEmpty(yAdjustment))
                    {
                        if (float.TryParse(yAdjustment, NumberStyles.Float, CultureInfo.InvariantCulture, out float yAdj))
                        {
                            _yPositionAdjustment = yAdj;
                        }
                        else
                        {
                            Debug.LogError($"[{gameObject.name}] Failed to parse TMP Y adjustment value: '{yAdjustment}'");
                            _yPositionAdjustment = 0f;
                        }
                    }
                    else
                    {
                        _yPositionAdjustment = 0f;
                    }

                    // Apply bold if needed
                    string boldNewFontBool = LocTableCache.FontsTable.GetString("Bold");
                    Boolean.TryParse(boldNewFontBool, out _boldNewFont);

                    // Apply scale adjustment
                    if (allowScaleAdjustment)
                    {
                        float newMinSize = origMinSize * _newFontScaleAdjustment;
                        float newMaxSize = origMaxSize * _newFontScaleAdjustment;

                        Debug.Log($"[{gameObject.name}] Scaling TMP font: Original min={origMinSize}, max={origMaxSize}, " +
                                  $"Scale={_newFontScaleAdjustment}, New min={newMinSize}, max={newMaxSize}");

                        _tmpText.fontSizeMin = newMinSize;
                        _tmpText.fontSizeMax = newMaxSize;
                    }

                    // Apply Y position adjustment
                    if (allowPositionAdjustment && _yPositionAdjustment != 0)
                    {
                        Vector3 newPosition = _originalPosition;
                        newPosition.y += _yPositionAdjustment;
                        transform.localPosition = newPosition;
                        Debug.Log($"[{gameObject.name}] Adjusting TMP Y position: Original Y={_originalPosition.y}, " +
                                  $"Adjustment={_yPositionAdjustment}, New Y={newPosition.y}");
                    }
                    else if (allowPositionAdjustment)
                    {
                        // Reset to original position
                        transform.localPosition = _originalPosition;
                    }
                }

                UpdateInspectorInfo();
            }
            catch (Exception e)
            {
                Debug.Log($"font localisation failed - UpdateString: {e.Message}");
            }
        }

        private Font GetLocaleSpecificFont(string localeName, string currentFontName)
        {
            // Debug.Log($"[{gameObject.name}] GetLocaleSpecificFont - Locale: {localeName}, Current Font: {currentFontName}");

            // Check if the current font is MechaWeka
            bool isMechaWeka = currentFontName.Contains("MechaWeka", StringComparison.OrdinalIgnoreCase);
            // Check if the current font is Casalear
            bool isCasalear = currentFontName.Contains("Casalear", StringComparison.OrdinalIgnoreCase);

            // Debug.Log($"[{gameObject.name}] Font type check - IsMechaWeka: {isMechaWeka}, IsCasalear: {isCasalear}");

            // If neither MechaWeka nor Casalear, return null to use the default font
            if (!isMechaWeka && !isCasalear)
            {
                Debug.Log($"[{gameObject.name}] No matching font type found for: {currentFontName}");
                return null;
            }

            // Determine the font suffix based on the current font
            string fontSuffix = isMechaWeka ? "Heading" : "Regular";

            // Determine the language prefix based on the locale
            string languagePrefix = GetLanguagePrefix(localeName);

            // Debug.Log($"[{gameObject.name}] Attempting to load font - Prefix: {languagePrefix}, Suffix: {fontSuffix}");

            // If we have a valid language prefix, load the appropriate font
            if (!string.IsNullOrEmpty(languagePrefix))
            {
                string fontName = $"{languagePrefix}{fontSuffix}";
                // Debug.Log($"[{gameObject.name}] Trying to load font: {fontName}");

                // Try loading from Resources/Fonts
                Font font = Resources.Load<Font>($"Fonts/{fontName}");
                if (font == null)
                {
                    Debug.LogWarning($"[{gameObject.name}] Failed to load font {fontName} from any location. Using default font: {currentFontName}");
                    // Return the current font instead of null to prevent font loss
                    return _text.font;
                }
                else
                {
                    // Debug.Log($"[{gameObject.name}] Successfully loaded font: {fontName}");
                }

                return font;
            }

            // Debug.Log($"[{gameObject.name}] No language prefix found for locale: {localeName}");
            return null; // Return null to use the default font
        }

        private string GetLanguagePrefix(string localeName)
        {
            if (localeName.Contains("Korean") || localeName.Contains("ko"))
            {
                return "Korean";
            }
            else if (localeName.Contains("Japanese") || localeName.Contains("ja"))
            {
                return "Japanese";
            }
            else if (localeName.Contains("Chinese") || localeName.Contains("zh"))
            {
                return "Chinese";
            }
            else if (localeName.Contains("Arabic") || localeName.Contains("ar"))
            {
                return "Arabic";
            }
            else if (localeName.Contains("Thai") || localeName.Contains("th"))
            {
                return "Thai";
            }
            else if (localeName.Contains("Russian") || localeName.Contains("ru"))
            {
                return "Cyrillic";
            }

            return string.Empty; // Return empty string for unsupported languages
        }

        private TMP_FontAsset GetLocaleSpecificTmpFont(string localeName)
        {
            // Debug.Log($"[{gameObject.name}] GetLocaleSpecificTmpFont - Locale: {localeName}");

            // Determine the language prefix based on the locale
            string languagePrefix = GetLanguagePrefix(localeName);

            // Debug.Log($"[{gameObject.name}] TMP Font - Language prefix: {languagePrefix}");

            // If we have a valid language prefix, load the appropriate TMP font
            if (!string.IsNullOrEmpty(languagePrefix))
            {
                string fontName = $"{languagePrefix}Regular";
                //Debug.Log($"[{gameObject.name}] Trying to load TMP font: {fontName}");

                // Try loading from Resources/Fonts
                TMP_FontAsset font = Resources.Load<TMP_FontAsset>($"Fonts/{fontName} SDF");
                if (font == null)
                {
                    Debug.LogWarning($"[{gameObject.name}] Failed to load TMP font {fontName} from any location. Using default font: {_tmpText.font.name}");
                    // Return the current font instead of null to prevent font loss
                    return _tmpText.font;
                }
                else
                {
                    // Debug.Log($"[{gameObject.name}] Successfully loaded TMP font: {fontName}");
                }

                return font;
            }

            // Debug.Log($"[{gameObject.name}] No language prefix found for TMP font, locale: {localeName}");
            return null; // Return null to use the default TMP font
        }

        private void ConvertToRtl()
        {
            if (LocalizationSettings.SelectedLocale.LocaleName == "Arabic (ar)" && _text != null)
            {
                _rtlSettings.IsRtl = true;
                var allCaps = GetComponent<AllCaps>();
                if (allCaps) allCaps.enabled = false;
                _rtlSettings.RtlCorrectedText = _text.text;
                _rtlSettings.RtlCorrectedText = ArabicSupport.Fix(_rtlSettings.RtlCorrectedText);
                _text.text = _rtlSettings.RtlCorrectedText;
            }
        }

        private void ResetRtl()
        {
            if (LocalizationSettings.SelectedLocale.LocaleName == "Arabic (ar)" && _text != null)
            {
                _text.text = _rtlSettings.RtlCorrectedText;
            }
        }
    }

    // Add this attribute to make inspector fields read-only
    public class ReadOnlyAttribute : PropertyAttribute { }
}
