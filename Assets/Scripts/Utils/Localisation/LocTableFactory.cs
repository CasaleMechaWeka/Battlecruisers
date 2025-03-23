using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace BattleCruisers.Utils.Localisation
{

    public static class TableName
    {
        public const string BATTLE_SCENE = "BattleScene";
        public const string COMMON = "Common";
        public const string SCREENS_SCENE = "ScreensScene";
        public const string STORY = "StoryTable";
        public const string TUTORIAL = "Tutorial";
        public const string FONTS = "Fonts";
        public const string ADVERTISING = "Advertising";
        public const string HECKLES = "Heckles";
    }

    public static class LocTableFactory
    {
        private static LocTable _common, _battleScene, _screensScene, _story, _tutorial, _fonts, _advertising, _heckles;


        public static LocTable CommonTable => _common ??= LoadTable(TableName.COMMON);
        public static LocTable BattleSceneTable => _battleScene ??= LoadTable(TableName.BATTLE_SCENE);
        public static LocTable ScreensSceneTable => _screensScene ??= LoadTable(TableName.SCREENS_SCENE);
        public static LocTable StoryTable => _story ??= LoadTable(TableName.STORY);
        public static LocTable TutorialTable => _tutorial ??= LoadTable(TableName.TUTORIAL);
        public static LocTable FontsTable => _fonts ??= LoadTable(TableName.FONTS);
        public static LocTable AdvertisingTable => _advertising ??= LoadTable(TableName.ADVERTISING);
        public static LocTable HecklesTable => _heckles ??= LoadTable(TableName.HECKLES);

        public static async Task PreloadAllAsync()
        {
            await Task.WhenAll(
                LoadTableAsync(TableName.COMMON, t => _common = t),
                LoadTableAsync(TableName.BATTLE_SCENE, t => _battleScene = t),
                LoadTableAsync(TableName.SCREENS_SCENE, t => _screensScene = t),
                LoadTableAsync(TableName.STORY, t => _story = t),
                LoadTableAsync(TableName.TUTORIAL, t => _tutorial = t),
                LoadTableAsync(TableName.FONTS, t => _fonts = t),
                LoadTableAsync(TableName.ADVERTISING, t => _advertising = t),
                LoadTableAsync(TableName.HECKLES, t => _heckles = t)
            );
        }

        private static LocTable LoadTable(string tableName)
        {
            if (!LocalizationSettings.InitializationOperation.IsDone)
                LocalizationSettings.InitializationOperation.WaitForCompletion();

            Locale locale = LocalizationSettings.SelectedLocale;

            var handle = LocalizationSettings.StringDatabase.GetTableAsync(tableName, locale);
            handle.WaitForCompletion();

            Assert.IsTrue(handle.Status == AsyncOperationStatus.Succeeded);
            Assert.IsNotNull(handle.Result);

            return new LocTable(handle);
        }

        public static async Task LoadTableAsync(string tableName, System.Action<LocTable> assign = null)
        {
            await LocalizationSettings.InitializationOperation.Task;
            Locale locale = LocalizationSettings.SelectedLocale;

            var handle = LocalizationSettings.StringDatabase.GetTableAsync(tableName, locale);
            await handle.Task;

            Assert.IsTrue(handle.Status == AsyncOperationStatus.Succeeded);
            Assert.IsNotNull(handle.Result);

            if (assign != null)
                assign(new LocTable(handle));
        }

        public static void ReleaseAll()
        {
            Release(ref _common);
            Release(ref _battleScene);
            Release(ref _screensScene);
            Release(ref _story);
            Release(ref _tutorial);
            Release(ref _fonts);
            Release(ref _advertising);
            Release(ref _heckles);
        }

        private static void Release(ref LocTable table)
        {
            if (table != null)
            {
                Addressables.Release(table.Handle);
                table = null;
            }
        }
    }
}