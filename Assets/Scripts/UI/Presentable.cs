using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.UI
{
	public interface IPresentable
	{
		// About to be shown
		void OnPresenting();

		// About to be hidden
		void OnDismissing();
	}

	public class Presentable : MonoBehaviour, IPresentable
	{
		protected IList<IPresentable> _childPresentables;

		public virtual void Initialize()
		{
			_childPresentables = new List<IPresentable>();
		}

		public virtual void OnPresenting()
		{
			foreach (IPresentable presentable in _childPresentables)
			{
				presentable.OnPresenting();
			}
		}

		public virtual void OnDismissing()
		{
			foreach (IPresentable presentable in _childPresentables)
			{
				presentable.OnDismissing();
			}
		}
	}
}
