using BattleCruisers.Buildables;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.TargetFinders
{
	public interface IFactionObjectFilter
	{
		bool IsMatch(IFactionable factionObject);
	}
}
