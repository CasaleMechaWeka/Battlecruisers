using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Targets.TargetProcessors
{
	/// <summary>
	/// Ranks all targets, and assigns the highest ranked target to ITargetConsumers.
	/// </summary>
	public interface ITargetProcessor : IDisposable
	{
		/// <exception cref="ArgumentException">If the target consumer is already added.</exception>
		void AddTargetConsumer(ITargetConsumer targetConsumer);

		/// <exception cref="ArgumentException">If the target consumer was not added first.</exception>
		void RemoveTargetConsumer(ITargetConsumer targetConsumer);
	}
}
