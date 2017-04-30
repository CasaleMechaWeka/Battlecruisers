using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.TargetFinders.Filters
{
	public class UnitFilter : FactionFilter
	{
		private readonly UnitCategory _unitCategory;

		public UnitFilter(Faction faction, UnitCategory unitCategory) : base(faction)
		{
			_unitCategory = unitCategory;
		}

		public override bool IsMatch(IFactionable factionObject)
		{
			bool isMatch = false;

			if (base.IsMatch(factionObject))
			{
				IUnit unit = factionObject as IUnit;

				if (unit != null && unit.Category == _unitCategory)
				{
					isMatch = true;
				}
			}
			
			return isMatch;
		}
	}
}
