using System;
using UnityEngine.Assertions;
using UnityEngine.Localization.Pseudo;
using UnityEngine.Localization.Settings;
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
            //Assert.IsTrue(Handle.IsValid(), $"Handle has been released :/");

            StringTableEntry entry = Handle.Result.GetEntry(key);
            Assert.IsNotNull(entry, $"No string entry for key: {key}");

#if !PSEUDO_LOCALE
            return entry.GetLocalizedString();
#else
            if (LocalizationSettings.SelectedLocale is PseudoLocale loc)
            {
                return loc.GetPseudoString(entry.GetLocalizedString());
            }
            else
            {
                throw new Exception($"Selected locale is not pseudo locale :/  {LocalizationSettings.SelectedLocale}");
            }
#endif
        }
    }
}