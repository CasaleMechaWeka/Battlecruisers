using BattleCruisers.UI.Sound;
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

        public ITextDisplayer TextDisplayer { get; private set; }
        public IExplanationDismissButton OkButton { get; private set; }
        public IExplanationDismissButton DoneButton { get; private set; }

        public void Initialise(ISoundPlayer soundPlayer)
        {
            TextDisplayer textDisplayer = GetComponentInChildren<TextDisplayer>(includeInactive: true);
            Assert.IsNotNull(textDisplayer);
            textDisplayer.Initialise();
            TextDisplayer = textDisplayer;

            ExplanationDismissButton okButton = transform.FindNamedComponent<ExplanationDismissButton>("OkButton");
            okButton.Initialise(soundPlayer);
            OkButton = okButton;

            ExplanationDismissButton doneButton = transform.FindNamedComponent<ExplanationDismissButton>("DoneButton");
            doneButton.Initialise(soundPlayer);
            DoneButton = doneButton;

            _transform = transform.Parse<RectTransform>();
        }

        public void Expand()
        {
            _transform.sizeDelta = new Vector2(EXPANDED_HEIGHT, _transform.sizeDelta.y);
        }

        public void Shrink()
        {
            _transform.sizeDelta = new Vector2(SHRUNK_HEIGHT, _transform.sizeDelta.y);
        }
    }
}