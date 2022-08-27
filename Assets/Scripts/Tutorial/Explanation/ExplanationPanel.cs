using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Explanation
{
    public class ExplanationPanel : MonoBehaviourWrapper, IExplanationPanel
    {
        private RectTransform _transform;

        private const float LARGE_HEIGHT = 770;
        private const float EXPANDED_HEIGHT = 570; 
        private const float EXPANDED_HEIGHT_NO_BUTTONS = 460;
        private const float MID_HEIGHT = 430;
        private const float MID_HEIGHT_NO_BUTTONS = 320;
        private const float SHRUNK_HEIGHT = 220;

        public TextDisplayer textDisplayer;
        public ITextDisplayer TextDisplayer => textDisplayer;

        public ExplanationDismissButton okButton;
        public IExplanationDismissButton OkButton => okButton;

        public ExplanationDismissButton doneButton;
        public IExplanationDismissButton DoneButton => doneButton;

        public void Initialise(ISingleSoundPlayer soundPlayer)
        {
            Helper.AssertIsNotNull(textDisplayer, okButton, doneButton);
            Assert.IsNotNull(soundPlayer);

            textDisplayer.Initialise(gameObject);

            okButton.Initialise(soundPlayer);
            OkButton.Enabled = false;

            doneButton.Initialise(soundPlayer);
            DoneButton.Enabled = false;

            _transform = transform.Parse<RectTransform>();
        }

        public void ExpandHeight()
        {
            Logging.LogMethod(Tags.TUTORIAL_EXPLANATION_PANEL);
            //change height depending on the length of the text (number of characters)
            if (textDisplayer.Text.Length <= 36)
            {
                _transform.sizeDelta = new Vector2(_transform.sizeDelta.x, MID_HEIGHT_NO_BUTTONS);
            }
            else if (textDisplayer.Text.Length < 60)
            {
                if (!doneButton.Enabled && !okButton.Enabled) //if no buttons then reduce height further
                {
                    _transform.sizeDelta = new Vector2(_transform.sizeDelta.x, MID_HEIGHT_NO_BUTTONS);
                }
                else
                {
                    _transform.sizeDelta = new Vector2(_transform.sizeDelta.x, MID_HEIGHT);
                }
            }
            else if (textDisplayer.Text.Length < 140)
            {
                if (!doneButton.Enabled && !okButton.Enabled) //if no buttons then reduce height further
                {
                    _transform.sizeDelta = new Vector2(_transform.sizeDelta.x, EXPANDED_HEIGHT_NO_BUTTONS);
                }
                else
                {
                    _transform.sizeDelta = new Vector2(_transform.sizeDelta.x, EXPANDED_HEIGHT);
                }
            }
            else 
            {
                _transform.sizeDelta = new Vector2(_transform.sizeDelta.x, LARGE_HEIGHT);
            }
            
        }

        public void ShrinkHeight()
        {
            Logging.LogMethod(Tags.TUTORIAL_EXPLANATION_PANEL);
            _transform.sizeDelta = new Vector2(_transform.sizeDelta.x, SHRUNK_HEIGHT);
        }
    }
}