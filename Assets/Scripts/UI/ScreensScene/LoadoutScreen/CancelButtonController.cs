using BattleCruisers.UI.Sound;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public class CancelButtonController : CanvasGroupButton
    {
        private ICancellable _cancellable;

        public void Initialise(ISoundPlayer soundPlayer, ICancellable cancellable)
        {
            base.Initialise(soundPlayer);

            Assert.IsNotNull(cancellable);
            _cancellable = cancellable;
        }

        protected override void OnClicked()
        {
            base.OnClicked();
            _cancellable.Cancel();
        }
    }
}