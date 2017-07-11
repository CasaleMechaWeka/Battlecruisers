using BattleCruisers.Buildables;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using BattleCruisers.Utils;

namespace BattleCruisers.Movement
{
	public interface IHomingMovementController
	{
		ITarget Target { set; }
		void AdjustVelocity();
	}
}
