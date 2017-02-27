using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnitDetector : UnitDetector 
{
	public UnitType OwnType { private get; set; }

	protected override bool ShouldTriggerOnEntered(IUnit unit)
	{
		return ShouldTrigger(unit);
	}

	protected override bool ShouldTriggerOnExited(IUnit unit)
	{
		return ShouldTrigger(unit);
	}

	private bool ShouldTrigger(IUnit unit)
	{
		return unit.Type != OwnType;
	}
}
