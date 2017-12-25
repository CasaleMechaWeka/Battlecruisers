using System.Collections.Generic;
using BattleCruisers.Buildables.Repairables;
using UnityEngine;

namespace BattleCruisers.Buildables
{
    public enum Faction
	{
		Blues, Reds
	}

	public enum TargetType
	{
		Aircraft, Ships, Cruiser, Buildings, Rocket
	}

	/// <summary>
	/// Used for prioritising targets, so do NOT change order!
	/// </summary>
	public enum TargetValue
	{
		Low, Medium, High
	}

    public interface ITarget : IDamagable, IRepairable
	{
		Faction Faction { get; }
		TargetType TargetType { get; }
		Vector2 Velocity { get; }
		List<TargetType> AttackCapabilities { get; }
		TargetValue TargetValue { get; }
		Vector2 Position { get; }
	}
}
