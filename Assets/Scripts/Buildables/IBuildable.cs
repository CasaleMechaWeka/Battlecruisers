using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.UI;
using BattleCruisers.UI.ProgressBars;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using BattleCruisers.Targets;

namespace BattleCruisers.Buildables
{
	public enum BuildableState
	{
		NotStarted, InProgress, Paused, Completed
	}

	public class BuildProgressEventArgs : EventArgs
	{
		/// <summary>
		/// 0-1
		/// </summary>
		public float BuildProgress { get; private set; }

		public BuildProgressEventArgs(float buildProgress)
		{
			BuildProgress = buildProgress;
		}
	}

	public interface IBuildable
	{
		BuildableState BuildableState { get; }
		float Damage { get; }
		Vector3 Size { get; }
		Sprite Sprite { get; }
		IDroneConsumer DroneConsumer { get; }

		event EventHandler StartedConstruction;
		event EventHandler CompletedBuildable;
		event EventHandler<BuildProgressEventArgs> BuildableProgress;

		void Initialise(Faction faction, UIManager uiManager, ICruiser parentCruiser, ICruiser enemyCruiser, BuildableFactory buildableFactory, ITargetsFactory targetsFactory);
		void StartConstruction();
		void InitiateDelete();
	}
}
