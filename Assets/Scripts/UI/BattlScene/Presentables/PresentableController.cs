namespace BattleCruisers.UI.BattleScene.Presentables
{
    public abstract class PresentableController : ClickableTogglable, IPresentable
	{
        private IPresentableComponent _presentableComponent;

		protected bool IsPresented => _presentableComponent.IsPresented;

		public override void Initialise()
		{
            base.Initialise();

            _presentableComponent = new PresentableComponent();
		}

		public virtual void OnPresenting(object activationParameter)
		{
            _presentableComponent.OnPresenting(activationParameter);
		}

		public virtual void OnDismissing()
		{
            _presentableComponent.OnDismissing();
		}

        protected void AddChildPresentable(IPresentable presentableToAdd)
        {
            _presentableComponent.AddChildPresentable(presentableToAdd);
        }

        protected override void OnClicked()
        {
            // Empty, do not force child classes to implement
        }
    }
}
