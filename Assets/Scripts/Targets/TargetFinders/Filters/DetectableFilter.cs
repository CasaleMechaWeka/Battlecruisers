using BattleCruisers.Buildables;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleCruisers.Targets.TargetFinders.Filters
{
	public class DetectableFilter : FactionAndTargetTypeFilter
	{
		private readonly bool _isDetectable;

		public DetectableFilter(Faction faction, bool isDetectable, params TargetType[] targetTypes)
			: base(faction, targetTypes)
		{
			_isDetectable = isDetectable;
		}

		public override bool IsMatch(ITarget target)
		{
			return base.IsMatch(target) && target.IsDetectable == _isDetectable;
		}
	}
}
