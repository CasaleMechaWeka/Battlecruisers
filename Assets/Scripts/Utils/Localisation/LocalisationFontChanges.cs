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
        public Font newFont;
        public Font defaultFont;

        // Start is called before the first frame update
        void Start()
        {
            _text = GetComponent<Text>();
            newFont = (Font)Resources.Load("Fonts/NotoSansCJK-VF.ttf");
            defaultFont = _text.font;//default font is what ever was originally there

            Assert.IsNotNull(_text, $"{gameObject.name}: {nameof(AllCaps)} should only be attached to a game object that has a {nameof(Text)} element.");

            _localizeStringEvent = GetComponent<LocalizeStringEvent>();
            Assert.IsNotNull(_localizeStringEvent, $"{gameObject.name}: {nameof(LocalisationFontChanges)} should only be attached to a game object that has a {nameof(LocalizeStringEvent)} element.");

            _localizeStringEvent.OnUpdateString.AddListener(OnUpdateString);
            _localizeStringEvent.RefreshString();
        }

        private void OnUpdateString(string localisedString)
        {
            _text.text = localisedString.ToUpper();

            Locale currentSelectedLocale = LocalizationSettings.SelectedLocale;
            ILocalesProvider availableLocales = LocalizationSettings.AvailableLocales;

            if (currentSelectedLocale == availableLocales.GetLocale("ja"))
            {
                if (newFont != null)
                {
                    _text.font = newFont;
                    _text.fontStyle = FontStyle.Bold;
                }
            }
            else
            {
                if (defaultFont != null)
                {
                    _text.font = defaultFont;
                    _text.fontStyle = FontStyle.Normal;
                }
            }
        }
    }
}
