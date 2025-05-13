using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using BattleCruisers.UI;
using BattleCruisers.UI.Commands;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using System;
using System.Collections.ObjectModel;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables
{
    public enum PvPBuildableState
    {
        NotStarted, InProgress, Paused, Completed
    }

    public class PvPBuildProgressEventArgs : EventArgs
    {
        public IPvPBuildable Buildable { get; }

        public PvPBuildProgressEventArgs(IPvPBuildable buildable)
        {
            Buildable = buildable;
        }
    }

    public interface IPvPBuildable : ITarget, IComparableItem, IClickableEmitter
    {
        /// <summary>
        /// 0-1
        /// </summary>
        float BuildProgress { get; }
        // string outlineName { get; set; }
        PvPBuildableState BuildableState { get; }
        int NumOfDronesRequired { get; }
        float BuildTimeInS { get; }
        IDroneConsumer DroneConsumer { get; }
        ICommand ToggleDroneConsumerFocusCommand { get; }
        float CostInDroneS { get; }
        ReadOnlyCollection<IDamageCapability> DamageCapabilities { get; }
        IBoostable BuildProgressBoostable { get; }
        bool IsInitialised { get; }
        IPvPCruiser ParentCruiser { get; set; }
        IPvPCruiser EnemyCruiser { get; }
        PvPHealthBarController HealthBar { get; }
        string PrefabName { get; }
        string keyName { get; set; }
        event EventHandler StartedConstruction;
        event EventHandler CompletedBuildable;
        event EventHandler<PvPBuildProgressEventArgs> BuildableProgress;
        event EventHandler<DroneNumChangedEventArgs> DroneNumChanged;

        void StaticInitialise(GameObject parent, PvPHealthBarController healthBar);
        void Initialise();
        void Initialise(PvPUIManager uiManager);
        void StartConstruction();
    }
}