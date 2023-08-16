using System.Linq;
using System.Threading.Tasks;
using BattleCruisers.Data;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
using UnityEngine.Localization;
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

        private static Locale _locale;

        public class TableName
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

        private ILocTable _battleSceneTable, _commonTable, _screensSceneTable, _storyTable, _tutorialTable, _hecklesTable, _fonts, _advertisingTable;

        private LocTableFactory() { }

        public async Task<ILocTable> LoadFontsTableAsync()
        {
            if (_fonts == null)
            {
                AsyncOperationHandle<StringTable> tableHandle = await LoadTable(TableName.FONTS);
                _fonts = new LocTable(tableHandle);
            }

            return _fonts;
        }

        public async Task<ILocTable> LoadBattleSceneTableAsync()
        {
            if (_battleSceneTable == null)
            {
                AsyncOperationHandle<StringTable> tableHandle = await LoadTable(TableName.BATTLE_SCENE);
                _battleSceneTable = new LocTable(tableHandle);
            }

            return _battleSceneTable;
        }

        public async Task<ILocTable> LoadScreensSceneTableAsync()
        {
            if (_screensSceneTable == null)
            {
                AsyncOperationHandle<StringTable> tableHandle = await LoadTable(TableName.SCREENS_SCENE);
                _screensSceneTable = new LocTable(tableHandle);
            }

            return _screensSceneTable;
        }

        public async Task<ILocTable> LoadCommonTableAsync()
        {
            if (_commonTable == null)
            {
                AsyncOperationHandle<StringTable> tableHandle = await LoadTable(TableName.COMMON);
                _commonTable = new LocTable(tableHandle);
            }

            return _commonTable;
        }

        public async Task<ILocTable> LoadStoryTableAsync()
        {
            //Debug.Log("Loaded story");
            if (_storyTable == null)
            {
                AsyncOperationHandle<StringTable> tableHandle = await LoadTable(TableName.STORY);
                _storyTable = new LocTable(tableHandle);
            }

            return _storyTable;
        }

        public async Task<ILocTable> LoadTutorialTableAsync()
        {
            if (_tutorialTable == null)
            {
                AsyncOperationHandle<StringTable> tableHandle = await LoadTable(TableName.TUTORIAL);
                _tutorialTable = new LocTable(tableHandle);
            }

            return _tutorialTable;
        }

        public async Task<ILocTable> LoadAdvertisingTableAsync()
        {
            if (_advertisingTable == null)
            {
                AsyncOperationHandle<StringTable> tableHandle = await LoadTable(TableName.ADVERTISING);
                _advertisingTable = new LocTable(tableHandle);
            }

            return _advertisingTable;
        }


        public async Task<ILocTable> LoadHecklesTableAsync()
        {
            if (_hecklesTable == null)
            {
                AsyncOperationHandle<StringTable> tableHandle = await LoadTable(TableName.HECKLES);
                _hecklesTable = new LocTable(tableHandle);
            }

            return _hecklesTable;
        }


        private async Task<AsyncOperationHandle<StringTable>> LoadTable(string tableName)
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
        private async Task<Locale> GetLocaleAsync()
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

        public void ReleaseFontsTable()
        {
            if (_fonts != null)
            {
                Addressables.Release(_fonts.Handle);
                _fonts = null;
            }
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

        public void ReleaseTutorialTable()
        {
            if (_tutorialTable != null)
            {
                Addressables.Release(_tutorialTable.Handle);
                _tutorialTable = null;
            }
        }

        public void ReleaseAdvertisingTable()
        {
            if (_advertisingTable != null)
            {
                Addressables.Release(_tutorialTable.Handle);
                _advertisingTable = null;
            }
        }

        public void ReleaseHecklesTable()
        {
            if (_hecklesTable != null)
            {
                Addressables.Release(_hecklesTable.Handle);
                _hecklesTable = null;
            }
        }
    }
}