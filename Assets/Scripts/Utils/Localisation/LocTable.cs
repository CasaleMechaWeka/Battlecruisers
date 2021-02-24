using UnityEngine.Assertions;
using UnityEngine.Localization.Tables;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace BattleCruisers.Utils.Localisation
{
    public class LocTable : ILocTable
    {
        public AsyncOperationHandle<StringTable> Handle { get; }

        public LocTable(AsyncOperationHandle<StringTable> tableHandle)
        {
            Assert.IsTrue(tableHandle.IsValid());
            Handle = tableHandle;
        }

        public string GetString(string key)
        {
            Assert.IsFalse(string.IsNullOrEmpty(key));
            Assert.IsTrue(Handle.IsValid(), $"Handle has been released :/");

            StringTableEntry entry = Handle.Result.GetEntry(key);
            Assert.IsNotNull(entry, $"No string entry for key: {key}");

            return entry.GetLocalizedString();
        }
    }
}