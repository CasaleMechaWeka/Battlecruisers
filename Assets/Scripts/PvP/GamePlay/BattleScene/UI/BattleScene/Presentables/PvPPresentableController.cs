using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Presentables
{
    public abstract class PvPPresentableController : MonoBehaviourWrapper, IPvPPresentable
    {
        private IPvPPresentableComponent _presentableComponent;

        public event EventHandler Dismissed;

        public bool IsPresented => _presentableComponent.IsPresented;

        public void Initialise()
        {
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

        protected void AddChildPresentable(IPvPPresentable presentableToAdd)
        {
            _presentableComponent.AddChildPresentable(presentableToAdd);
        }
    }
}
