using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Explanation
{
    public class ExplanationPanel : MonoBehaviourWrapper, IExplanationPanel
    {
        private RectTransform _transform;
        private RectTransform _textDisplayerTransform;

        private const float LARGE_HEIGHT = 770;
        private const float EXPANDED_HEIGHT = 570; 
        private const float EXPANDED_HEIGHT_NO_BUTTONS = 460;
        private const float MID_HEIGHT = 430;
        private const float MID_HEIGHT_NO_BUTTONS = 320;
        private const float SHRUNK_HEIGHT = 220;
        
        private const float TEXT_RECT_BOTTOM_NO_BUTTON = 0;
        private const float TEXT_RECT_BOTTOM_BUTTON = 150;

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
            _textDisplayerTransform = textDisplayer.transform.Parse<RectTransform>();
        }

        public void ExpandHeight()
        {
            Logging.LogMethod(Tags.TUTORIAL_EXPLANATION_PANEL);
            //change height depending on the length of the text (number of characters) and change the bottom of the textDisplayer rect to match the window
            if (textDisplayer.Text.Length < 39)
            {
                if (!doneButton.Enabled && !okButton.Enabled)
                {
                    _transform.sizeDelta = new Vector2(_transform.sizeDelta.x, MID_HEIGHT_NO_BUTTONS);
                    _textDisplayerTransform.offsetMin = new Vector2(_textDisplayerTransform.offsetMin.x, TEXT_RECT_BOTTOM_NO_BUTTON);
                }
                else
                {
                    _transform.sizeDelta = new Vector2(_transform.sizeDelta.x, MID_HEIGHT);
                    _textDisplayerTransform.offsetMin = new Vector2(_textDisplayerTransform.offsetMin.x, TEXT_RECT_BOTTOM_BUTTON);
                }
            }
            else if (textDisplayer.Text.Length < 60)
            {
                if (!doneButton.Enabled && !okButton.Enabled) //if no buttons then reduce height further
                {
                    _transform.sizeDelta = new Vector2(_transform.sizeDelta.x, MID_HEIGHT_NO_BUTTONS);
                    _textDisplayerTransform.offsetMin = new Vector2(_textDisplayerTransform.offsetMin.x, TEXT_RECT_BOTTOM_NO_BUTTON);
                }
                else
                {
                    _transform.sizeDelta = new Vector2(_transform.sizeDelta.x, MID_HEIGHT);
                    _textDisplayerTransform.offsetMin = new Vector2(_textDisplayerTransform.offsetMin.x, TEXT_RECT_BOTTOM_BUTTON);
                }
            }
            else if (textDisplayer.Text.Length < 140)
            {
                if (!doneButton.Enabled && !okButton.Enabled) //if no buttons then reduce height further
                {
                    _transform.sizeDelta = new Vector2(_transform.sizeDelta.x, EXPANDED_HEIGHT_NO_BUTTONS);
                    _textDisplayerTransform.offsetMin = new Vector2(_textDisplayerTransform.offsetMin.x, TEXT_RECT_BOTTOM_NO_BUTTON);
                }
                else
                {
                    _transform.sizeDelta = new Vector2(_transform.sizeDelta.x, EXPANDED_HEIGHT);
                    _textDisplayerTransform.offsetMin = new Vector2(_textDisplayerTransform.offsetMin.x, TEXT_RECT_BOTTOM_BUTTON);
                }
            }
            else 
            {
                _transform.sizeDelta = new Vector2(_transform.sizeDelta.x, LARGE_HEIGHT);
                _textDisplayerTransform.offsetMin = new Vector2(_textDisplayerTransform.offsetMin.x, TEXT_RECT_BOTTOM_BUTTON);
            }
            
        }

        public void ShrinkHeight()
        {
            Logging.LogMethod(Tags.TUTORIAL_EXPLANATION_PANEL);
            _transform.sizeDelta = new Vector2(_transform.sizeDelta.x, SHRUNK_HEIGHT);
            _textDisplayerTransform.offsetMin = new Vector2(_textDisplayerTransform.offsetMin.x, TEXT_RECT_BOTTOM_NO_BUTTON);
        }
    }
}