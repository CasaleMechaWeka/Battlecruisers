using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.UI.BattleScene
{
	public interface IPresentable
	{
		// About to be shown
		void OnPresenting(object activationParameter);

		// About to be hidden
		void OnDismissing();
	}

    public class Presentable : UIElement, IPresentable
	{
		protected bool _isPresented;
		protected IList<IPresentable> _childPresentables;

		public override void Initialise()
		{
            base.Initialise();
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
