using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.TargetFinders.Filters
{
	public interface ITargetFilterFactory
	{
		ITargetFilter CreateTargetFilter(Faction faction, params TargetType[] targetTypes);
	}

	public class TargetFilterFactory : ITargetFilterFactory
	{
		public ITargetFilter CreateTargetFilter(Faction faction, params TargetType[] targetTypes)
		{
			return new TargetFilter(faction, targetTypes);
		}
	}
}
