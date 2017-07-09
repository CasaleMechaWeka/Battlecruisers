using System;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval
{
	public interface IFireIntervalProvider
	{
		float NextFireIntervalInS { get; }
	}
}
