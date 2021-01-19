using UnityEngine.ResourceManagement.AsyncOperations;

namespace BattleCruisers.Utils.Fetchers
{
    public interface IPrefabContainer<TPrefab> where TPrefab : class
    {
        AsyncOperationHandle<TPrefab> Handle { get; }
        TPrefab Prefab { get; }
    }
}