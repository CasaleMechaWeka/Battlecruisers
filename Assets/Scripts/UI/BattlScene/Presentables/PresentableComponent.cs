using System;
using System.Collections.Generic;

namespace BattleCruisers.UI.BattleScene.Presentables
{
    public sealed class PresentableComponent : IPresentableComponent
    {
        private readonly IList<IPresentable> _childPresentables;

        // FELIX  Update tests :)
        public event EventHandler Presented;
        public event EventHandler Dismissed;

        public bool IsPresented { get; private set; }

        public PresentableComponent()
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

            Presented?.Invoke(this, EventArgs.Empty);
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
