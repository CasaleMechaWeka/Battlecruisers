using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace BattleCruisers.Utils.Localisation
{
    public class LocTableFactory : ILocTableFactory
    {
        private static ILocTableFactory _instance;
        public static ILocTableFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LocTableFactory();
                }
                return _instance;
            }
        }

        public class TableName
        {
            public const string BATTLE_SCENE = "BattleScene";
            public const string COMMON = "Common";
            public const string SCREENS_SCENE = "ScreensScene";
            public const string STORY = "StoryTable";
        }

        private ILocTable _battleSceneTable, _commonTable, _screensSceneTable, _storyTable;

        private LocTableFactory() { }

        public async Task<ILocTable> LoadBattleSceneTable()
        {
            if (_battleSceneTable == null)
            {
                AsyncOperationHandle<StringTable> tableHandle = await LoadTable(TableName.BATTLE_SCENE);
                _battleSceneTable = new LocTable(tableHandle);
            }

            return _battleSceneTable;
        }

        public async Task<ILocTable> LoadScreensSceneTable()
        {
            if (_screensSceneTable == null)
            {
                AsyncOperationHandle<StringTable> tableHandle = await LoadTable(TableName.SCREENS_SCENE);
                _screensSceneTable = new LocTable(tableHandle);
            }

            return _screensSceneTable;
        }

        public async Task<ILocTable> LoadCommonTable()
        {
            if (_commonTable == null)
            {
                AsyncOperationHandle<StringTable> tableHandle = await LoadTable(TableName.COMMON);
                _commonTable = new LocTable(tableHandle);
            }

            return _commonTable;
        }

        public async Task<ILocTable> LoadStoryTable()
        {
            if (_storyTable == null)
            {
                AsyncOperationHandle<StringTable> tableHandle = await LoadTable(TableName.STORY);
                _storyTable = new LocTable(tableHandle);
            }

            return _storyTable;
        }

        private async Task<AsyncOperationHandle<StringTable>> LoadTable(string tableName)
        {
            AsyncOperationHandle<StringTable> handle = LocalizationSettings.StringDatabase.GetTableAsync(tableName);

            // Load table, so getting any strings will be synchronous
            await handle.Task;

            Assert.IsTrue(handle.Status == AsyncOperationStatus.Succeeded);
            Assert.IsNotNull(handle.Result);

            return handle;
        }

        public void ReleaseBattleSceneTable()
        {
            if (_battleSceneTable != null)
            {
                Addressables.Release(_battleSceneTable.Handle);
                _battleSceneTable = null;
            }
        }

        public void ReleaseScreensSceneTable()
        {
            if (_screensSceneTable != null)
            {
                Addressables.Release(_screensSceneTable.Handle);
                _screensSceneTable = null;
            }
        }

        public void ReleaseCommonTable()
        {
            if (_commonTable != null)
            {
                Addressables.Release(_commonTable.Handle);
                _commonTable = null;
            }
        }

        public void ReleaseStoryTable()
        {
            if (_storyTable != null)
            {
                Addressables.Release(_storyTable.Handle);
                _storyTable = null;
            }
        }
    }
}