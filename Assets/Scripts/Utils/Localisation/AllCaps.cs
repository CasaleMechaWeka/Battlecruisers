using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace BattleCruisers.Utils.Localisation
{
    public class AllCaps : MonoBehaviour
    {
        private Text _text;
        private LocalizeStringEvent _localizeStringEvent;

        void Start()
        {
            _text = GetComponent<Text>();
            //Debug.Log(_text.text + "- Start");
            Assert.IsNotNull(_text, $"{gameObject.name}: {nameof(AllCaps)} should only be attached to a game object that has a {nameof(Text)} element.");
            
            _localizeStringEvent = GetComponent<LocalizeStringEvent>();
            Assert.IsNotNull(_localizeStringEvent, $"{gameObject.name}: {nameof(AllCaps)} should only be attached to a game object that has a {nameof(LocalizeStringEvent)} element.");
            
            if (LocalizationSettings.SelectedLocale.LocaleName != "Arabic (ar)")
            {
                _localizeStringEvent.OnUpdateString.AddListener(OnUpdateString);
                _localizeStringEvent.RefreshString();
            }
            else if (LocalizationSettings.SelectedLocale.LocaleName == "Arabic (ar)")
            {
                if (gameObject.GetComponent<LocalisationFontChanges>() == null) gameObject.AddComponent<LocalisationFontChanges>();
            }
        }

        private void OnUpdateString(string localisedString)
        {
            _text.text = localisedString.ToUpper();
            //Debug.Log(_text.text + "- OnUpdateString");
        }
    }
}