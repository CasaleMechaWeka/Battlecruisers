using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public class CancelButtonController : ClickableTogglable
    {
        private ICancellable _cancellable;

        private CanvasGroup _canvasGroup;
        protected override CanvasGroup CanvasGroup => _canvasGroup;

        public void Initialise(ICancellable cancellable)
        {
            base.Initialise();

            Assert.IsNotNull(cancellable);
            _cancellable = cancellable;

            _canvasGroup = GetComponent<CanvasGroup>();
            Assert.IsNotNull(_canvasGroup);
        }

        protected override void OnClicked()
        {
            _cancellable.Cancel();
        }
    }
}