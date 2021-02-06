using BattleCruisers.Scenes;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.Loading
{
    public class LoadingScreenController : MonoBehaviour
    {
        public Canvas root;
        public Text loadingText;

        private const string DEFAULT_LOADING_TEXT = "Loading";
        private const string LEGAL_TEXT = "Any unauthorized exhibition, distribution, or copying of this video game or any part thereof (including soundtrack) may result in civil liability and criminal prosecution. The story, names, characters, and incidents portrayed in this production are fictitious. No identification with actual persons (living or deceased), places, buildings, and products is intended or should be inferred.";

        private static bool IsFirstTime = true;
        public static LoadingScreenController Instance { get; private set; }

        void Start()
        {
            Helper.AssertIsNotNull(root, loadingText);

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
                return LandingSceneGod.LoadingScreenHint ?? DEFAULT_LOADING_TEXT;
            }
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}