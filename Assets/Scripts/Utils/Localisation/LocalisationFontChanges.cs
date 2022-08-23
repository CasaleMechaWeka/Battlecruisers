using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace BattleCruisers.Utils.Localisation
{
    public class LocalisationFontChanges : MonoBehaviour
    {
        private Text _text;
        private LocalizeStringEvent _localizeStringEvent;
        private Font _newFont;
        private int _originalFontMaxSize;
        private int _originalFontMinSize;
        private float _newFontScaleAdjustment;
        private bool _boldNewFont;
        public bool allowScaleAdjustment = true;

        // Start is called before the first frame update
        async void Start()
        {

            //get original settings incase we need to switch back
            _text = GetComponent<Text>();
            Assert.IsNotNull(_text, $"{gameObject.name}: {nameof(AllCaps)} should only be attached to a game object that has a {nameof(Text)} element.");

            _originalFontMaxSize = _text.resizeTextMaxSize;
            _originalFontMinSize = _text.resizeTextMinSize;
            
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

        private void UpdateString()
        {
            Locale currentSelectedLocale = LocalizationSettings.SelectedLocale;
            ILocalesProvider availableLocales = LocalizationSettings.AvailableLocales;

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
    }
}
