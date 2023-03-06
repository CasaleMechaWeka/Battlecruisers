using System;
using System.Globalization;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using Utils.Localisation;

namespace BattleCruisers.Utils.Localisation
{
    public class LocalisationFontChanges : MonoBehaviour
    {
        private Text _text;
        private Font _newFont;
        private int _originalFontMaxSize;
        private int _originalFontMinSize;
        private float _newFontScaleAdjustment;
        private bool _boldNewFont;
        private LocalizeStringEvent _localizeStringEvent;
        private RTLSettings _rtlSettings = new RTLSettings();
        public bool allowScaleAdjustment = true;

        public struct RTLSettings
        {
            public bool IsRtl { get; set; }
            public string RtlCorrectedText { get; set; }
        }

        private void Awake()
        {
            
            if (_text == null) _text = GetComponent<Text>();
            
            if (TryGetComponent(out LocalizeStringEvent localizeStringEvent))
            {
                _localizeStringEvent = localizeStringEvent;
            }
            else if (transform.parent.TryGetComponent(out LocalizeStringEvent localizeStringEventInParent))
            {
                _localizeStringEvent = localizeStringEventInParent;
            }
        }

        // Start is called before the first frame update
        async void Start()
        {

            try
            {
                //get original settings incase we need to switch back
                Assert.IsNotNull(_text, $"{gameObject.name}: {nameof(AllCaps)} should only be attached to a game object that has a {nameof(Text)} element.");

                _originalFontMaxSize = _text.resizeTextMaxSize;
                _originalFontMinSize = _text.resizeTextMinSize;

                _rtlSettings.IsRtl = false;
                _rtlSettings.RtlCorrectedText = "";

                ILocTable fontSettings = await LocTableFactory.Instance.LoadFontsTableAsync();

                //get the string we need
                string newFontName = fontSettings.GetString("FontName");
                _newFont = (Font)Resources.Load("Fonts/" + newFontName);
                string bestFitScaleAdjustment = fontSettings.GetString("BestFitScaleAdjustment");
                _newFontScaleAdjustment = float.Parse(bestFitScaleAdjustment, CultureInfo.InvariantCulture.NumberFormat);
                string boldNewFontBool = fontSettings.GetString("Bold");
                Boolean.TryParse(boldNewFontBool, out _boldNewFont);
                UpdateString();
            }
            catch {
                Debug.Log("font localisation failed - Start");
                Debug.Log("NAna");
            }
        }

        private void OnEnable()
        {
            if (_localizeStringEvent != null)
            {
                _localizeStringEvent.OnUpdateString.AddListener(OnUpdateString);
                _localizeStringEvent.RefreshString();
            }
            else if (_text.text != _rtlSettings.RtlCorrectedText && _rtlSettings.IsRtl == false)
            {
                ConvertToRtl();
            }
            else if (_text.text != _rtlSettings.RtlCorrectedText && _rtlSettings.IsRtl)
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
            _text.text = localizedString;
            ConvertToRtl();
        }

        private async void UpdateString()
        {
            try
            {
                //if there is a new font use it
                if (_newFont != null)
                {
                    _text.font = _newFont;
                }
                if (_boldNewFont)
                {
                    _text.fontStyle = FontStyle.Bold;
                }
                if (allowScaleAdjustment)//controlled when the component is applied
                {   //default scale adjustment is 1.0 e.g. no adjustment
                    _text.resizeTextMinSize = (int)(_originalFontMinSize * _newFontScaleAdjustment);
                    _text.resizeTextMaxSize = (int)(_originalFontMaxSize * _newFontScaleAdjustment);
                }
            }
            catch {
                Debug.Log("font localisation failed - UpdateString");
            }
        }

        private async void ConvertToRtl()
        {
            if (LocalizationSettings.SelectedLocale.LocaleName == "Arabic (ar)")
            {
                _rtlSettings.IsRtl = true;
                await Task.Delay(1);
                var allCaps = GetComponent<AllCaps>();
                if (allCaps) allCaps.enabled = false;
                _rtlSettings.RtlCorrectedText = _text.text;
                _rtlSettings.RtlCorrectedText = ArabicSupport.Fix(_rtlSettings.RtlCorrectedText);
                _text.text = _rtlSettings.RtlCorrectedText;
            }
        }

        private async void ResetRtl()
        {
            if (LocalizationSettings.SelectedLocale.LocaleName == "Arabic (ar)")
            {
                await Task.Delay(1);
                _text.text = _rtlSettings.RtlCorrectedText;
            }
        }
    }
}
