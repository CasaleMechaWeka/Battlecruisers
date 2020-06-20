using System;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene.Presentables
{
    public abstract class PresentableController : ElementWithClickSound, IPresentable
	{
        private IPresentableComponent _presentableComponent;

        public event EventHandler Presented;
        public event EventHandler Dismissed;

        protected bool IsPresented => _presentableComponent.IsPresented;
        protected override ISoundKey ClickSound => null;

        public override void Initialise(ISingleSoundPlayer soundPlayer)
		{
            base.Initialise(soundPlayer);

            _presentableComponent = new PresentableComponent();
		}

		public virtual void OnPresenting(object activationParameter)
		{
            Logging.Verbose(Tags.UI, $"id: {gameObject.GetInstanceID()}  name: {gameObject.name}");
            _presentableComponent.OnPresenting(activationParameter);
            Presented?.Invoke(this, EventArgs.Empty);
		}

		public virtual void OnDismissing()
		{
            Logging.Verbose(Tags.UI, $"id: {gameObject.GetInstanceID()}  name: {gameObject.name}");
            _presentableComponent.OnDismissing();
            Dismissed?.Invoke(this, EventArgs.Empty);
		}

        protected void AddChildPresentable(IPresentable presentableToAdd)
        {
            _presentableComponent.AddChildPresentable(presentableToAdd);
        }
    }
}
