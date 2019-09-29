using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Explanation
{
    public class ExplanationPanel : MonoBehaviourWrapper, IExplanationPanel
    {
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
        }
    }
}