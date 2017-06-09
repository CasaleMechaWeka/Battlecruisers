using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Drones;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.ProgressBars;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using BattleCruisers.Targets;

namespace BattleCruisers.Cruisers
{
	// FELIX  Use event for units?  Ie, in case it's a big experimental unit?
	public class StartedConstructionEventArgs : EventArgs
	{
		public IBuildable Buildable { get; private set; }

		public StartedConstructionEventArgs(IBuildable buildable)
		{
			Buildable = buildable;
		}
	}

	public interface ICruiser : ITarget
	{
		BuildingWrapper SelectedBuildingPrefab { get; set; }
		IDroneManager DroneManager { get; }
		IDroneConsumerProvider DroneConsumerProvider { get; }
		Direction Direction { get; }
		Vector2 Size { get; }

		event EventHandler<StartedConstructionEventArgs> StartedConstruction;

		bool IsSlotAvailable(SlotType slotType);
		void HighlightAvailableSlots(SlotType slotType);
		void UnhighlightSlots();
		Building ConstructBuilding(BuildingWrapper buildingPrefab, ISlot slot);
		Building ConstructSelectedBuilding(ISlot slot);
	}
}
