using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetProcessors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleCruisers.Targets.TargetFinders.Filters
{
	public interface IExactMatchTargetFilter : ITargetFilter, ITargetConsumer { }

	public class ExactMatchTargetFilter : IExactMatchTargetFilter
	{
		public ITarget Target { private get; set; }

		public virtual bool IsMatch(ITarget target)
		{
			return object.ReferenceEquals(Target, target);
		}
	}
}
