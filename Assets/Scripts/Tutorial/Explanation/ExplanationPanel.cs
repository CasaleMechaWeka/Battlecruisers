using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Explanation
{
    public class ExplanationPanel : MonoBehaviourWrapper, IExplanationPanel
    {
        public ITextDisplayer TextDisplayer { get; private set; }
        public IExplanationDismissButton DismissButton { get; private set; }

        public void Initialise()
        {
            TextDisplayer textDisplayer = GetComponentInChildren<TextDisplayer>(includeInactive: true);
            Assert.IsNotNull(textDisplayer);
            textDisplayer.Initialise();
            TextDisplayer = textDisplayer;

            DismissButton = GetComponentInChildren<IExplanationDismissButton>(includeInactive: true);
            Assert.IsNotNull(DismissButton);
        }
    }
}