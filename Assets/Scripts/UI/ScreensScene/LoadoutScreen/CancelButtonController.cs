using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public class CancelButtonController : CanvasGroupButton
    {
        private ICancellable _cancellable;

        public void Initialise(ICancellable cancellable)
        {
            base.Initialise();

            Assert.IsNotNull(cancellable);
            _cancellable = cancellable;
        }

        protected override void OnClicked()
        {
            _cancellable.Cancel();
        }
    }
}