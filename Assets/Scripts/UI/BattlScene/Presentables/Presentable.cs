namespace BattleCruisers.UI.BattleScene.Presentables
{
    public class Presentable : IPresentable
	{
        private readonly IPresentableComponent _presentableComponent;

        public Presentable()
		{
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
