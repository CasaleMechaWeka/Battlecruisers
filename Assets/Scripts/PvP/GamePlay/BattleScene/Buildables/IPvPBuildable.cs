using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Commands;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.LoadoutScreen.Comparisons;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Utils.Localisation;
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

    public interface IPvPBuildable : IPvPTarget, IPvPComparableItem, IPvPClickableEmitter
    {
        /// <summary>
        /// 0-1
        /// </summary>
        float BuildProgress { get; }
        PvPBuildableState BuildableState { get; }
        int NumOfDronesRequired { get; }
        float BuildTimeInS { get; }
        IPvPDroneConsumer DroneConsumer { get; }
        IPvPCommand ToggleDroneConsumerFocusCommand { get; }
        float CostInDroneS { get; }
        ReadOnlyCollection<IPvPDamageCapability> DamageCapabilities { get; }
        IPvPBoostable BuildProgressBoostable { get; }
        bool IsInitialised { get; }
        IPvPCruiser ParentCruiser { get; }
        IPvPCruiser EnemyCruiser { get; }
        IPvPHealthBar HealthBar { get; }
        string PrefabName { get; }
        string keyName { get; set; }
        event EventHandler StartedConstruction;
        event EventHandler CompletedBuildable;
        event EventHandler<PvPBuildProgressEventArgs> BuildableProgress;
        event EventHandler<PvPDroneNumChangedEventArgs> DroneNumChanged;

        void StaticInitialise(GameObject parent, PvPHealthBarController healthBar, ILocTable commonStrings);
        void Initialise(IPvPFactoryProvider factoryProvider);
        void StartConstruction();
    }
}