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
            _text = GetComponent<Text>();
            Assert.IsNotNull(_text);
            _text.text = string.Empty;
        }

        public void DisplayText(string textToDisplay)
        {
            _text.text = textToDisplay;
        }
    }
}
