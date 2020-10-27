using BattleCruisers.Utils;
using System;

namespace BattleCruisers.UI.BattleScene.Presentables
{
    public abstract class PresentableController : MonoBehaviourWrapper, IPresentable
    {
        private IPresentableComponent _presentableComponent;

        public event EventHandler Dismissed;

        public bool IsPresented => _presentableComponent.IsPresented;

        public void Initialise()
        {
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
            Dismissed?.Invoke(this, EventArgs.Empty);
        }

        protected void AddChildPresentable(IPresentable presentableToAdd)
        {
            _presentableComponent.AddChildPresentable(presentableToAdd);
        }
    }
}
