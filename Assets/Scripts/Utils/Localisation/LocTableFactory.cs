using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace BattleCruisers.Utils.Localisation
{
    public class LocTableFactory
    {
        public class TableName
        {
            public const string SCREENS_SCENE = "ScreensScene";
        }

        private ILocTable _screensSceneTable, _battleSceneTable, _commonTable;

        public async Task<ILocTable> LoadScreensSceneTable()
        {
            if (_screensSceneTable == null)
            {
                AsyncOperationHandle<StringTable> tableHandle = await LoadTable(TableName.SCREENS_SCENE);
                _screensSceneTable = new LocTable(tableHandle);
            }

            return _screensSceneTable;

        }

        public void ReleaseScreensSceneTable()
        {
            if (_screensSceneTable != null)
            {
                Addressables.Release(_screensSceneTable.Handle);
                _screensSceneTable = null;
            }
        }
        
        public async Task<AsyncOperationHandle<StringTable>> LoadTable(string tableName)
        {
            AsyncOperationHandle<StringTable> handle = LocalizationSettings.StringDatabase.GetTableAsync(tableName);

            // Load table, so getting any strings will be synchronous
            await handle.Task;

            Assert.IsTrue(handle.Status == AsyncOperationStatus.Succeeded);
            Assert.IsNotNull(handle.Result);

            return handle;
        }
    }
}