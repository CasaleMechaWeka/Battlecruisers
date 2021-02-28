using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Assets.Scripts.Utils.Localisation
{
    public class SelectPseudoLocale : MonoBehaviour
    {
        public Locale pseudoLocale;

        private void Awake()
        {
            Assert.IsNotNull(pseudoLocale);
            LocalizationSettings.SelectedLocale = pseudoLocale;
        }
    }
}