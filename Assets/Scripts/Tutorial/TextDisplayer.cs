using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.Tutorial
{
    public class TextDisplayer : MonoBehaviour, ITextDisplayer
    {
        private Text _text;
        public string Text => _text.text;

        public event EventHandler TextChanged;

        public void Initialise()
        {
            _text = GetComponentInChildren<Text>();
            Assert.IsNotNull(_text);

            _text.text = string.Empty;
        }

        public void DisplayText(string textToDisplay)
        {
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }

            _text.text = textToDisplay;

            TextChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
