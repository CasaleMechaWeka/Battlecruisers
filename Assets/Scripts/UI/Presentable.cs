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
		public virtual void OnPresenting() { }
		public virtual void OnDismissing() { }
	}
}
