using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers
{
    public interface IPvPPrefabContainer<TPrefab> where TPrefab : class
    {
        AsyncOperationHandle<GameObject> Handle { get; }
        TPrefab Prefab { get; }
    }
}