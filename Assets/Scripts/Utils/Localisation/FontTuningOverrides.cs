using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace BattleCruisers.Utils.Localisation
{
    /// <summary>
    /// Dev-only runtime overrides for font tuning. This lets non-technical testers tweak font scale/Y
    /// and export a small JSON file you can copy back into the Localization table.
    /// </summary>
    public static class FontTuningOverrides
    {
        [Serializable]
        private class OverridesFile
        {
            public List<LocaleOverrides> locales = new List<LocaleOverrides>();
        }

        [Serializable]
        private class LocaleOverrides
        {
            public string localeName;
            public List<Kv> entries = new List<Kv>();
        }

        [Serializable]
        private class Kv
        {
            public string key;
            public string value;
        }

        private static readonly Dictionary<string, Dictionary<string, string>> _cache =
            new Dictionary<string, Dictionary<string, string>>(StringComparer.Ordinal);

        private static bool _loaded;

        public static string OverridesPath =>
            Path.Combine(Application.persistentDataPath, "font_tuning_overrides.json");

        public static void EnsureLoaded()
        {
            if (_loaded) return;
            _loaded = true;
            LoadFromDisk();
        }

        public static bool TryGetString(string key, out string value)
        {
            EnsureLoaded();

            value = null;
            if (string.IsNullOrEmpty(key)) return false;
            if (LocalizationSettings.SelectedLocale == null) return false;

            string localeName = LocalizationSettings.SelectedLocale.LocaleName;
            if (!_cache.TryGetValue(localeName, out var dict)) return false;

            // Try exact key first.
            if (dict.TryGetValue(key, out value)) return true;

            // Defensive: tolerate accidental trailing space in some Localization keys.
            if (!key.EndsWith(" ", StringComparison.Ordinal) && dict.TryGetValue(key + " ", out value)) return true;
            if (key.EndsWith(" ", StringComparison.Ordinal) && dict.TryGetValue(key.TrimEnd(' '), out value)) return true;

            return false;
        }

        public static void SetStringForCurrentLocale(string key, string value)
        {
            EnsureLoaded();
            if (LocalizationSettings.SelectedLocale == null) return;
            string localeName = LocalizationSettings.SelectedLocale.LocaleName;

            if (!_cache.TryGetValue(localeName, out var dict))
            {
                dict = new Dictionary<string, string>(StringComparer.Ordinal);
                _cache[localeName] = dict;
            }

            dict[key] = value ?? "";
        }

        public static void SetFloatForCurrentLocale(string key, float value)
        {
            SetStringForCurrentLocale(key, value.ToString(CultureInfo.InvariantCulture));
        }

        public static void SetBoolForCurrentLocale(string key, bool value)
        {
            SetStringForCurrentLocale(key, value ? "true" : "false");
        }

        public static void ClearCurrentLocale()
        {
            EnsureLoaded();
            if (LocalizationSettings.SelectedLocale == null) return;
            string localeName = LocalizationSettings.SelectedLocale.LocaleName;
            _cache.Remove(localeName);
        }

        public static void SaveToDisk()
        {
            EnsureLoaded();
            try
            {
                var file = new OverridesFile();
                foreach (var pair in _cache)
                {
                    var locale = new LocaleOverrides { localeName = pair.Key };
                    foreach (var kv in pair.Value)
                    {
                        locale.entries.Add(new Kv { key = kv.Key, value = kv.Value });
                    }
                    file.locales.Add(locale);
                }

                string json = JsonUtility.ToJson(file, true);
                Directory.CreateDirectory(Path.GetDirectoryName(OverridesPath) ?? Application.persistentDataPath);
                File.WriteAllText(OverridesPath, json);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Failed to save font tuning overrides: {e.Message}");
            }
        }

        public static string ExportJsonString()
        {
            EnsureLoaded();
            try
            {
                var file = new OverridesFile();
                foreach (var pair in _cache)
                {
                    var locale = new LocaleOverrides { localeName = pair.Key };
                    foreach (var kv in pair.Value)
                    {
                        locale.entries.Add(new Kv { key = kv.Key, value = kv.Value });
                    }
                    file.locales.Add(locale);
                }
                return JsonUtility.ToJson(file, true);
            }
            catch
            {
                return "{}";
            }
        }

        private static void LoadFromDisk()
        {
            try
            {
                if (!File.Exists(OverridesPath)) return;
                string json = File.ReadAllText(OverridesPath);
                if (string.IsNullOrWhiteSpace(json)) return;

                var file = JsonUtility.FromJson<OverridesFile>(json);
                if (file?.locales == null) return;

                _cache.Clear();
                foreach (var locale in file.locales)
                {
                    if (string.IsNullOrEmpty(locale?.localeName)) continue;
                    var dict = new Dictionary<string, string>(StringComparer.Ordinal);
                    if (locale.entries != null)
                    {
                        foreach (var entry in locale.entries)
                        {
                            if (string.IsNullOrEmpty(entry?.key)) continue;
                            dict[entry.key] = entry.value ?? "";
                        }
                    }
                    _cache[locale.localeName] = dict;
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Failed to load font tuning overrides: {e.Message}");
            }
        }
    }
}


