using UnityEngine;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;
using UnityEngine.Assertions;
using UnityEngine.Localization.Tables;

namespace BattleCruisers.Utils.Localisation
{
    public class Loc
    {
        public class TableName
        {
            public const string SCREENS_SCENE = "ScreensScene";
        }

        public async Task LoadTable(string table)
        {
            AsyncOperationHandle<StringTable> handle = LocalizationSettings.StringDatabase.GetTableAsync(table);

            await handle.Task;
        }

        public async Task<string> GetScreensSceneStringAsync(string key)
        {
            return await GetStringAsync(TableName.SCREENS_SCENE, key);
        }

        // FELIX  How to force string table to load programmatically?  don't want to preload all tables :/  Only want to load that scene's table
        //  FELIX  Make static?
        // FELIX  Don't make async.  Will preload all strings :)
        private async Task<string> GetStringAsync(string tableName, string key)
        {
            Assert.IsFalse(string.IsNullOrEmpty(key));

            AsyncOperationHandle<string> handle = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(tableName, key);

            // FELIX  Should have loaded table already => should be done
            Assert.IsTrue(handle.IsDone);

            await handle.Task;

            if (handle.Status != AsyncOperationStatus.Succeeded
                || handle.Result == null)
            {
                throw new ArgumentException($"Failed to retrieve string: {key}  from table: {tableName}");
            }

            return handle.Result;
        }
    }
}