using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene.Presentables
{
    public abstract class PresentableController : ElementWithClickSound, IPresentable
	{
        private IPresentableComponent _presentableComponent;

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
		}

		public virtual void OnDismissing()
		{
            Logging.Verbose(Tags.UI, $"id: {gameObject.GetInstanceID()}  name: {gameObject.name}");
            _presentableComponent.OnDismissing();
		}

        protected void AddChildPresentable(IPresentable presentableToAdd)
        {
            _presentableComponent.AddChildPresentable(presentableToAdd);
        }
    }
}
