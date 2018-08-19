using System.Collections.Generic;

namespace BattleCruisers.UI.BattleScene.Presentables
{
    public class Presentable : IPresentable
	{
		protected bool _isPresented;
		protected readonly IList<IPresentable> _childPresentables;

        public Presentable()
		{
			_childPresentables = new List<IPresentable>();
		}

		public virtual void OnPresenting(object activationParameter)
		{
			_isPresented = true;

			foreach (IPresentable presentable in _childPresentables)
			{
				presentable.OnPresenting(activationParameter);
			}
		}

		public virtual void OnDismissing()
		{
			_isPresented = false;

			foreach (IPresentable presentable in _childPresentables)
			{
				presentable.OnDismissing();
			}
		}
	}
}
