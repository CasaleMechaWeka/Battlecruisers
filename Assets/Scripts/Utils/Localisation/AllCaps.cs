using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Localization.Components;
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
            Debug.Log(_text.text + "- Start");
            Assert.IsNotNull(_text, $"{gameObject.name}: {nameof(AllCaps)} should only be attached to a game object that has a {nameof(Text)} element.");

            _localizeStringEvent = GetComponent<LocalizeStringEvent>();
            Assert.IsNotNull(_localizeStringEvent, $"{gameObject.name}: {nameof(AllCaps)} should only be attached to a game object that has a {nameof(LocalizeStringEvent)} element.");

            _localizeStringEvent.OnUpdateString.AddListener(OnUpdateString);
            _localizeStringEvent.RefreshString();
        }

        private void OnUpdateString(string localisedString)
        {
            _text.text = localisedString.ToUpper();
            Debug.Log(_text.text + "- OnUpdateString");
        }
    }
}