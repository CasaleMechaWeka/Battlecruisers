using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.Tutorial.Steps
{
    public class TextDisplayer : MonoBehaviour, ITextDisplayer
    {
        private Text _text;

        public void Initialise()
        {
            _text = GetComponentInChildren<Text>();
            Assert.IsNotNull(_text);

            _text.text = string.Empty;
            gameObject.SetActive(true);
        }

        public void DisplayText(string textToDisplay)
        {
            _text.text = textToDisplay;
        }
    }
}
