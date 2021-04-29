using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Explanation
{
    public class ExplanationPanel : MonoBehaviourWrapper, IExplanationPanel
    {
        private RectTransform _transform;

        private const float EXPANDED_HEIGHT = 437;
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
            _transform.sizeDelta = new Vector2(_transform.sizeDelta.x, EXPANDED_HEIGHT);
        }

        public void ShrinkHeight()
        {
            Logging.LogMethod(Tags.TUTORIAL_EXPLANATION_PANEL);
            _transform.sizeDelta = new Vector2(_transform.sizeDelta.x, SHRUNK_HEIGHT);
        }
    }
}