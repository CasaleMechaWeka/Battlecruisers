using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Scenes;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;
using System;
using UnityEngine;
using Unity.Netcode;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using System.Threading.Tasks;
using BattleCruisers.Network.Multiplay.ApplicationLifecycle;
using BattleCruisers.Network.Multiplay.ConnectionManagement;
using BattleCruisers.Network.Multiplay.Gameplay.UI;
using BattleCruisers.Network.Multiplay.Infrastructure;
using BattleCruisers.Network.Multiplay.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene
{
    public class PvPBattleCompletionHandler : IPvPBattleCompletionHandler
    {
        private readonly IApplicationModel _applicationModel;
        private readonly ISceneNavigator _sceneNavigator;
        private PvPBattleSceneGodTunnel _battleSceneGodTunnel;
        private bool _isCompleted;

        public event EventHandler BattleCompleted;

        public PvPBattleCompletionHandler(
            IApplicationModel applicationModel,
            ISceneNavigator sceneNavigator,
            PvPBattleSceneGodTunnel battleSceneGodTunnel)
        {
            PvPHelper.AssertIsNotNull(applicationModel, sceneNavigator);
            _applicationModel = applicationModel;
            _sceneNavigator = sceneNavigator;

            _isCompleted = false;
            _battleSceneGodTunnel = battleSceneGodTunnel;
        }

        public async void CompleteBattle(bool wasVictory, bool retryLevel)
        {
            await Task.Delay(10);
            if (_isCompleted)
            {
                return;
            }
            _isCompleted = true;

            BattleCompleted?.Invoke(this, EventArgs.Empty);
            PvPBattleSceneGodClient.Instance.OnTunnelBattleCompleted_ValueChanged();
            if (NetworkManager.Singleton.IsConnectedClient)
                NetworkManager.Singleton.Shutdown(true);

            _applicationModel.DataProvider.SaveGame();

            _applicationModel.ShowPostBattleScreen = true;
            PvPTimeBC.Instance.TimeScale = 1;
        //    await GameObject.Find("ConnectionManager").GetComponent<ConnectionManager>().RequestShutdown();
            DestroyAllNetworkObjects();
            _sceneNavigator.GoToScene(PvPSceneNames.SCREENS_SCENE, true);
        }

        public async void CompleteBattle(bool wasVictory, bool retryLevel, long destructionScore)
        {
           await Task.Delay(10);
            if (_isCompleted)
            {
                return;
            }
            _isCompleted = true;
            BattleCompleted?.Invoke(this, EventArgs.Empty);
            PvPBattleSceneGodClient.Instance.OnTunnelBattleCompleted_ValueChanged();
            if (NetworkManager.Singleton.IsConnectedClient)
                NetworkManager.Singleton.Shutdown(true);

            _applicationModel.ShowPostBattleScreen = true;
            PvPTimeBC.Instance.TimeScale = 1;

            if (wasVictory)
            {
                if (SynchedServerData.Instance.GetTeam() == Cruisers.Team.LEFT)
                {
                    //Debug.Log(_applicationModel.DataProvider.GameModel.LifetimeDestructionScore + " - before");
                    _applicationModel.DataProvider.GameModel.LifetimeDestructionScore += destructionScore;
                    //Debug.Log(_applicationModel.DataProvider.GameModel.LifetimeDestructionScore + " - after");
                    if (_applicationModel.DataProvider.GameModel.BestDestructionScore < destructionScore)
                    {
                        _applicationModel.DataProvider.GameModel.BestDestructionScore = destructionScore;
                    }
                    _applicationModel.DataProvider.SaveGame();
                //    await GameObject.Find("ConnectionManager").GetComponent<ConnectionManager>().RequestShutdown();
                    DestroyAllNetworkObjects();
                    _sceneNavigator.GoToScene(PvPSceneNames.PvP_DESTRUCTION_SCENE, true);
                }
                else
                {
                //    await GameObject.Find("ConnectionManager").GetComponent<ConnectionManager>().RequestShutdown();
                    DestroyAllNetworkObjects();
                    _sceneNavigator.GoToScene(PvPSceneNames.SCREENS_SCENE, true);
                }
            }
            else
            {
                if (SynchedServerData.Instance.GetTeam() == Cruisers.Team.LEFT)
                {
                //    await GameObject.Find("ConnectionManager").GetComponent<ConnectionManager>().RequestShutdown();
                    DestroyAllNetworkObjects();
                    _sceneNavigator.GoToScene(PvPSceneNames.SCREENS_SCENE, true);
                }
                else
                {
                    //Debug.Log(_applicationModel.DataProvider.GameModel.LifetimeDestructionScore + " - before");
                    _applicationModel.DataProvider.GameModel.LifetimeDestructionScore += destructionScore;
                    //Debug.Log(_applicationModel.DataProvider.GameModel.LifetimeDestructionScore + " - after");
                    if (_applicationModel.DataProvider.GameModel.BestDestructionScore < destructionScore)
                    {
                        _applicationModel.DataProvider.GameModel.BestDestructionScore = destructionScore;
                    }
                    _applicationModel.DataProvider.SaveGame();
                //    await GameObject.Find("ConnectionManager").GetComponent<ConnectionManager>().RequestShutdown();
                    DestroyAllNetworkObjects();
                    _sceneNavigator.GoToScene(PvPSceneNames.PvP_DESTRUCTION_SCENE, true);
                }
            }
        }

        public async void DestroyAllNetworkObjects()
        {
            //await GameObject.Find("ConnectionManager").GetComponent<ConnectionManager>().RequestShutdown();
            await Task.Delay(10);
            GameObject.Find("ApplicationController").GetComponent<ApplicationController>().DestroyNetworkObject();
            GameObject.Find("ConnectionManager").GetComponent<ConnectionManager>().DestroyNetworkObject();
            GameObject.Find("PopupPanelManager").GetComponent<PopupManager>().DestroyNetworkObject();
            GameObject.Find("UIMessageManager").GetComponent<ConnectionStatusMessageUIManager>().DestroyNetworkObject();
            GameObject.Find("UpdateRunner").GetComponent<UpdateRunner>().DestroyNetworkObject();
            GameObject.Find("NetworkManager").GetComponent<BCNetworkManager>().DestroyNetworkObject();
        }
    }
}