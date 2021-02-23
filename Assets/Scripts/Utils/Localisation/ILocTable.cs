using UnityEngine.Localization.Tables;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace BattleCruisers.Utils.Localisation
{
    public interface ILocTable
    {
        AsyncOperationHandle<StringTable> Handle { get; }
        string GetString(string key);
    }
}