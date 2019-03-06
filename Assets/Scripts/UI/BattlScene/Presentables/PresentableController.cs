namespace BattleCruisers.UI.BattleScene.Presentables
{
    public class PresentableController : Togglable, IPresentable
	{
        private IPresentableComponent _presentableComponent;

		protected bool IsPresented { get { return _presentableComponent.IsPresented; } }

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
	}
}
