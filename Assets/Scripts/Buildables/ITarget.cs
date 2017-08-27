using System.Collections.Generic;
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
	/// Used for prioritising targets.  Can become a lot more fine graind :)
	/// </summary>
	public enum TargetValue
	{
		Low, Medium, High
	}

    public interface ITarget : IDamagable, IRepairable
	{
		Faction Faction { get; }
		TargetType TargetType { get; }
		bool IsDetectable { get; }
		Vector2 Velocity { get; }
		List<TargetType> AttackCapabilities { get; }
		TargetValue TargetValue { get; }
		Vector2 Position { get; }
	}
}
