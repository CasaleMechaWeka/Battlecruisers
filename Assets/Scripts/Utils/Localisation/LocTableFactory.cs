using System.Linq;
using System.Threading.Tasks;
using BattleCruisers.Data;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
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
        private static Locale _locale;

        private static ILocTable _battleSceneTable, _commonTable, _screensSceneTable, _storyTable, _tutorialTable, _hecklesTable, _fonts, _advertisingTable;

        public static async Task<ILocTable> LoadTableAsync(string tableName)
        {
            if (_fonts == null)
            {
                AsyncOperationHandle<StringTable> tableHandle = await LoadTable(tableName);
                _fonts = new LocTable(tableHandle);
            }

            return _fonts;
        }

        private static async Task<AsyncOperationHandle<StringTable>> LoadTable(string tableName)
        {
            Locale localeToUse = await GetLocaleAsync();
            //Debug.Log(localeToUse.name + " selected");
            AsyncOperationHandle<StringTable> handle = LocalizationSettings.StringDatabase.GetTableAsync(tableName, localeToUse);

            // Load table, so getting any strings will be synchronous
            await handle.Task;

            Assert.IsTrue(handle.Status == AsyncOperationStatus.Succeeded);
            Assert.IsNotNull(handle.Result);

            return handle;
        }

        //basically just need to make a string selection in settings menu and make it so that on load it uses the string in the code below
        //also need to setup drop down selector for this functionality
        //won't work until the game is fully translated
        private static async Task<Locale> GetLocaleAsync()
        {
            if (_locale != null)
            {
                Logging.Log(Tags.LOCALISATION, $"Returning stored locale: {_locale}");
                return _locale;
            }

            // Wait for locale preload to finish, otherwise accessing LocalizationSettings.AvailableLocales fails
            Locale localeToUse = await LocalizationSettings.SelectedLocaleAsync.Task;
            Logging.Log(Tags.LOCALISATION, $"Use pseudo loc");
            Locale arabic = LocalizationSettings.AvailableLocales.Locales.FirstOrDefault(locale => locale.name == "Arabic");

            //Debug.Log("Should be names below");
            foreach (Locale locale in LocalizationSettings.AvailableLocales.Locales)
            {
                //Debug.Log(locale.name);
            }

            if (ApplicationModelProvider.ApplicationModel.DataProvider.SettingsManager.Language != null)
            {
                foreach (Locale locale in LocalizationSettings.AvailableLocales.Locales)
                {
                    //Debug.Log(locale.name);
                    //replace below with the string saved in settings
                    if (locale.name == ApplicationModelProvider.ApplicationModel.DataProvider.SettingsManager.Language)
                    {

                        localeToUse = locale;
                        LocalizationSettings.SelectedLocale = localeToUse;
                        //ApplicationModelProvider.ApplicationModel.DataProvider.SettingsManager.Language = locale.name;
                    }
                }
            }
            else
            {
                ApplicationModelProvider.ApplicationModel.DataProvider.SettingsManager.Language = localeToUse.name;
                LocalizationSettings.SelectedLocale = localeToUse;
                //Debug.Log("Set the language to " + ApplicationModelProvider.ApplicationModel.DataProvider.SettingsManager.Language);
                //localeToUse = LocalizationSettings.SelectedLocale;
                //Debug.Log(localeToUse);
            }
            //localeToUse = Locale.CreateLocale(LocaleIdentifier);

            /*
            #if PSEUDO_LOCALE
                        Logging.Log(Tags.LOCALISATION, $"Use pseudo loc");
                        Locale pseudoLocale = LocalizationSettings.AvailableLocales.Locales.FirstOrDefault(locale => locale.name == "Pseudo-Locale(pseudo)");
                        Assert.IsNotNull(pseudoLocale);
                        LocalizationSettings.SelectedLocale = pseudoLocale;
                        localeToUse = pseudoLocale;
            #endif
            */



            _locale = localeToUse;
            //Debug.Log(_locale);
            return localeToUse;
        }

        public static void ReleaseFontsTable()
        {
            if (_fonts != null)
            {
                Addressables.Release(_fonts.Handle);
                _fonts = null;
            }
        }

        public static void ReleaseBattleSceneTable()
        {
            if (_battleSceneTable != null)
            {
                Addressables.Release(_battleSceneTable.Handle);
                _battleSceneTable = null;
            }
        }

        public static void ReleaseScreensSceneTable()
        {
            if (_screensSceneTable != null)
            {
                Addressables.Release(_screensSceneTable.Handle);
                _screensSceneTable = null;
            }
        }

        public static void ReleaseCommonTable()
        {
            if (_commonTable != null)
            {
                Addressables.Release(_commonTable.Handle);
                _commonTable = null;
            }
        }

        public static void ReleaseStoryTable()
        {
            if (_storyTable != null)
            {
                Addressables.Release(_storyTable.Handle);
                _storyTable = null;
            }
        }

        public static void ReleaseTutorialTable()
        {
            if (_tutorialTable != null)
            {
                Addressables.Release(_tutorialTable.Handle);
                _tutorialTable = null;
            }
        }

        public static void ReleaseAdvertisingTable()
        {
            if (_advertisingTable != null)
            {
                Addressables.Release(_tutorialTable.Handle);
                _advertisingTable = null;
            }
        }

        public static void ReleaseHecklesTable()
        {
            if (_hecklesTable != null)
            {
                Addressables.Release(_hecklesTable.Handle);
                _hecklesTable = null;
            }
        }
    }
}