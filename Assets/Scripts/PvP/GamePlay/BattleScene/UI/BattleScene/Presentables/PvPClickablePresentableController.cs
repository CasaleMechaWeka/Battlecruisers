using BattleCruisers.UI;
using BattleCruisers.UI.BattleScene.Presentables;
using BattleCruisers.UI.Sound;
using BattleCruisers.UI.Sound.Players;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Presentables
{
    public abstract class PvPClickablePresentableController : PvPElementWithClickSound, IPresentable
    {
        private IPresentableComponent _presentableComponent;

        public event EventHandler Dismissed;

        public bool IsPresented =>
            _presentableComponent != null
                && _presentableComponent.IsPresented;
        protected override SoundKey ClickSound => null;

        public void Initialise(ISingleSoundPlayer soundPlayer, IDismissableEmitter parent = null)
        {
            base.Initialise(soundPlayer, parent: parent);

            _presentableComponent = new PvPPresentableComponent();
        }

        public virtual void OnPresenting(object activationParameter)
        {
            // Logging.Verbose(Tags.UI, $"id: {gameObject.GetInstanceID()}  name: {gameObject.name}");
            _presentableComponent.OnPresenting(activationParameter);
        }

        public virtual void OnDismissing()
        {
            // Logging.Verbose(Tags.UI, $"id: {gameObject.GetInstanceID()}  name: {gameObject.name}");
            _presentableComponent.OnDismissing();
            Dismissed?.Invoke(this, EventArgs.Empty);
        }

        protected void AddChildPresentable(IPresentable presentableToAdd)
        {
            _presentableComponent.AddChildPresentable(presentableToAdd);
        }
    }
}
