using BattleCruisers.AI;
using BattleCruisers.Buildables.Colours;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Damage;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Data;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.Tutorial;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Clouds.Stats;
using BattleCruisers.UI.BattleScene.HelpLabels;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.UI.Music;
using BattleCruisers.UI.Sound;
using BattleCruisers.UI.Sound.Wind;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using BattleCruisers.Utils.BattleScene.Lifetime;
using BattleCruisers.Utils.Debugging;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Fetchers.Cache;
using BattleCruisers.Utils.Fetchers.Sprites;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using BattleCruisers.Utils.Threading;
using NSubstitute;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using BattleCruisers.Buildables;
using System.Collections.Generic;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Static;
using Unity.Services.Analytics;
using Unity.Netcode;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene
{
    public class PvPPlayerManager : NetworkBehaviour
    {
        IApplicationModel applicationModel;
        NetworkVariable<NetworkString> prefabPathOfCruiser = new NetworkVariable<NetworkString>(readPerm: NetworkVariableReadPermission.Everyone, writePerm: NetworkVariableWritePermission.Owner);

        private void Start()
        {

        }
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (IsClient)
            {
                applicationModel = ApplicationModelProvider.ApplicationModel;
                PrioritisedSoundKeys.SetSoundKeys(applicationModel.DataProvider.SettingsManager.AltDroneSounds);
            }
            else if (IsServer)
            {

            }
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
        }

        void Update()
        {
            // if (IsServer)
            // {
            //     if (prefabPathOfCruiser.Value.ToString().Split("=").Length == 2)
            //     {
            //         Debug.Log("Cruiser Name is " + prefabPathOfCruiser.Value.ToString().Split("=")[1]);
            //     }
            // }

        }
    }
}

