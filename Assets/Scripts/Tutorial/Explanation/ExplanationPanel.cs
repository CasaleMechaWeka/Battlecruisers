using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Explanation
{
    public class ExplanationPanel : MonoBehaviourWrapper, IExplanationPanel
    {
        public ITextDisplayer TextDisplayer { get; private set; }
        public IExplanationDismissButton OkButton { get; private set; }
        public IExplanationDismissButton DoneButton { get; private set; }

        public void Initialise()
        {
            TextDisplayer textDisplayer = GetComponentInChildren<TextDisplayer>(includeInactive: true);
            Assert.IsNotNull(textDisplayer);
            textDisplayer.Initialise();
            TextDisplayer = textDisplayer;

            OkButton = transform.FindNamedComponent<IExplanationDismissButton>("OkButton");
            DoneButton = transform.FindNamedComponent<IExplanationDismissButton>("DoneButton");
        }
    }
}