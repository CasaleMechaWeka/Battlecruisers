using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.Fetchers;
using BattleCruisers.Movement;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Utils;
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

		void Initialise(ICruiser parentCruiser, ICruiser enemyCruiser, UIManager uiManager, IFactoryProvider factoryProvider);
		void StartConstruction();
		void InitiateDelete();
	}
}
