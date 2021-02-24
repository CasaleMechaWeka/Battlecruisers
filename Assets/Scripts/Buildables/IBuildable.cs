using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.UI;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Commands;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.Localisation;
using System;
using System.Collections.ObjectModel;
using UnityEngine;

namespace BattleCruisers.Buildables
{
    public enum BuildableState
	{
		NotStarted, InProgress, Paused, Completed
	}

	public class BuildProgressEventArgs : EventArgs
	{
		public IBuildable Buildable { get; }

		public BuildProgressEventArgs(IBuildable buildable)
		{
			Buildable = buildable;
		}
	}

    public interface IBuildable : ITarget, IComparableItem, IClickableEmitter
    {
        /// <summary>
        /// 0-1
        /// </summary>
        float BuildProgress { get; }
        BuildableState BuildableState { get; }
		int NumOfDronesRequired { get; }
		float BuildTimeInS { get; }
		IDroneConsumer DroneConsumer { get; }
        ICommand ToggleDroneConsumerFocusCommand { get; }
        float CostInDroneS { get; }
        ReadOnlyCollection<IDamageCapability> DamageCapabilities { get; }
		IBoostable BuildProgressBoostable { get; }
        bool IsInitialised { get; }
        ICruiser ParentCruiser { get; }
        IHealthBar HealthBar { get; }
        string PrefabName { get; }

        event EventHandler StartedConstruction;
		event EventHandler CompletedBuildable;
		event EventHandler<BuildProgressEventArgs> BuildableProgress;
        event EventHandler<DroneNumChangedEventArgs> DroneNumChanged;

        void StaticInitialise(GameObject parent, HealthBarController healthBar, ILocTable commonStrings);
        void Initialise(IUIManager uiManager, IFactoryProvider factoryProvider);
        void StartConstruction();
        void InitiateDelete();
        void CancelDelete();
	}
}
