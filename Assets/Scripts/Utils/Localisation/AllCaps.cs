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
            Assert.IsNotNull(_text);

            _localizeStringEvent = GetComponent<LocalizeStringEvent>();
            Assert.IsNotNull(_localizeStringEvent);
        }
    }
}