using BattleCruisers.Buildables;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleCruisers.Targets.TargetFinders.Filters
{
	public interface ITargetFilter
	{
		bool IsMatch(ITarget target);
	}
}
