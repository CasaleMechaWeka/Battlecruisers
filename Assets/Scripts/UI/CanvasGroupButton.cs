using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI
{
    public abstract class CanvasGroupButton : ClickableTogglable
    {
        private CanvasGroup _canvasGroup;
        protected override CanvasGroup CanvasGroup => _canvasGroup;

        public override void Initialise()
        {
            base.Initialise();

            _canvasGroup = GetComponent<CanvasGroup>();
            Assert.IsNotNull(_canvasGroup);
        }
    }
}