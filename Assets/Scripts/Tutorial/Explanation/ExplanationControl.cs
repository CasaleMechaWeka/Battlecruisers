using BattleCruisers.UI;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Explanation
{
    // FELIX  Place control by item being highlighted :)
    public class ExplanationControl : Panel, IExplanationControl
    {
        private TextDisplayer _textDisplayer;
        private ClickableEmitter _dismissButton;

        public event EventHandler DismissButtonClicked;

        public void Initialise()
        {
            _textDisplayer = GetComponentInChildren<TextDisplayer>();
            Assert.IsNotNull(_textDisplayer);
            _textDisplayer.Initialise();

            _dismissButton = GetComponentInChildren<ClickableEmitter>();
            Assert.IsNotNull(_dismissButton);
            _dismissButton.Clicked += _dismissButton_Clicked;
        }

        private void _dismissButton_Clicked(object sender, EventArgs e)
        {
            if (DismissButtonClicked != null)
            {
                DismissButtonClicked.Invoke(this, EventArgs.Empty);
            }
        }

        public void DisplayText(string textToDisplay)
        {
            _textDisplayer.DisplayText(textToDisplay);
        }
    }
}