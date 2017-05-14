using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets;
using BattleCruisers.UI;
using BattleCruisers.UI.ProgressBars;
using BattleCruisers.Units.Aircraft.Providers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables
{
	public enum BuildableState
	{
		NotStarted, InProgress, Paused, Completed
	}

	public class BuildProgressEventArgs : EventArgs
	{
		public IBuildable Buildable { get; private set; }

		public BuildProgressEventArgs(IBuildable buildable)
		{
			Buildable = buildable;
		}
	}

	public interface IBuildable : ITarget
	{
		/// <summary>
		/// 0-1
		/// </summary>
		float BuildProgress { get; }
		BuildableState BuildableState { get; }
		float Damage { get; }
		Vector3 Size { get; }
		Sprite Sprite { get; }
		IDroneConsumer DroneConsumer { get; }

		event EventHandler StartedConstruction;
		event EventHandler CompletedBuildable;
		event EventHandler<BuildProgressEventArgs> BuildableProgress;

		void Initialise(Faction faction, UIManager uiManager, ICruiser parentCruiser, ICruiser enemyCruiser, 
			IBuildableFactory buildableFactory, ITargetsFactory targetsFactory, IAircraftProvider aircraftProvider);
		void StartConstruction();
		void InitiateDelete();
	}
}
