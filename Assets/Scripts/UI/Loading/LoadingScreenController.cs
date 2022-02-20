using BattleCruisers.Scenes;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.Loading
{
    public class LoadingScreenController : MonoBehaviour
    {
        public Canvas root;
        public Text loadingText;

        private string _defaultLoadingText;
        private const string LEGAL_TEXT = "Society grows great when old people plant trees in whose shade they know they shall never sit.";

        private static bool IsFirstTime = true;
        public static LoadingScreenController Instance { get; private set; }

        async void Start()
        {
            Helper.AssertIsNotNull(root, loadingText);

            ILocTable commonStrings = await LocTableFactory.Instance.LoadCommonTableAsync();
            _defaultLoadingText = commonStrings.GetString("UI/LoadingScreen/DefaultLoadingText");

            loadingText.text = FindLoadingText();
            Instance = this;

            IsFirstTime = false;
        }

        private string FindLoadingText()
        {
            if (IsFirstTime)
            {
                return LEGAL_TEXT;
            }
            else
            {
                return LandingSceneGod.LoadingScreenHint ?? _defaultLoadingText;
            }
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}