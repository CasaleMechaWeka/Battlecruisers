using System;
using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Presentables
{
    public sealed class PvPPresentableComponent : IPvPPresentableComponent
    {
        private readonly IList<IPvPPresentable> _childPresentables;

        public event EventHandler Dismissed;

        public bool IsPresented { get; private set; }

        public PvPPresentableComponent()
        {
            _childPresentables = new List<IPvPPresentable>();
        }

        public void OnPresenting(object activationParameter)
        {
            IsPresented = true;

            foreach (IPvPPresentable presentable in _childPresentables)
            {
                presentable.OnPresenting(activationParameter);
            }
        }

        public void OnDismissing()
        {
            IsPresented = false;

            foreach (IPvPPresentable presentable in _childPresentables)
            {
                presentable.OnDismissing();
            }

            Dismissed?.Invoke(this, EventArgs.Empty);
        }

        public void AddChildPresentable(IPvPPresentable presentableToAdd)
        {
            _childPresentables.Add(presentableToAdd);
        }
    }
}
