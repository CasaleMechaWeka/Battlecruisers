using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Assets.Scripts.Utils.Localisation
{
    // FELIX  Remove (or fix namespace :P)
    public class SelectPseudoLocale : MonoBehaviour
    {
        public Locale pseudoLocale;

        private void Awake()
        {
            // FELIX
            //Assert.IsNotNull(pseudoLocale);
            //LocalizationSettings.SelectedLocale = pseudoLocale;
        }
    }
}