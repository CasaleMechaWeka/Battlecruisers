using System;
using System.Collections.Generic;
using BattleCruisers.UI.BattleScene.Presentables;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Presentables
{
    public sealed class PvPPresentableComponent : IPresentableComponent
    {
        private readonly IList<IPresentable> _childPresentables;

        public event EventHandler Dismissed;

        public bool IsPresented { get; private set; }

        public PvPPresentableComponent()
        {
            _childPresentables = new List<IPresentable>();
        }

        public void OnPresenting(object activationParameter)
        {
            IsPresented = true;

            foreach (IPresentable presentable in _childPresentables)
            {
                presentable.OnPresenting(activationParameter);
            }
        }

        public void OnDismissing()
        {
            IsPresented = false;

            foreach (IPresentable presentable in _childPresentables)
            {
                presentable.OnDismissing();
            }

            Dismissed?.Invoke(this, EventArgs.Empty);
        }

        public void AddChildPresentable(IPresentable presentableToAdd)
        {
            _childPresentables.Add(presentableToAdd);
        }
    }
}
