using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace BattleCruisers.Utils.Localisation
{
    /// <summary>
    /// Dev-only overlay for non-technical font tuning.
    /// Toggle with F10. Adjust values, hit Apply, then Save/Copy JSON.
    /// </summary>
    public class FontTuningOverlay : MonoBehaviour
    {
        private bool _visible;
        private Vector2 _scroll;

        private float _bestFitScale = 1.0f;
        private float _headingScale = 1.0f;
        private float _regularScale = 1.0f;
        private float _headingY = 0f;
        private float _regularY = 0f;
        private bool _bold = false;

        private string _status = "";
        private float _statusUntil;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            FontTuningOverrides.EnsureLoaded();
            PullFromTablesOrOverrides();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F10))
                _visible = !_visible;

            if (_statusUntil > 0 && Time.unscaledTime > _statusUntil)
            {
                _statusUntil = 0;
                _status = "";
            }
        }

        private void OnGUI()
        {
            if (!_visible) return;

            GUI.depth = -1000;
            float w = Mathf.Min(560, Screen.width - 20);
            float h = Mathf.Min(650, Screen.height - 20);
            var rect = new Rect(10, 10, w, h);
            GUILayout.BeginArea(rect, GUI.skin.box);

            GUILayout.Label("Font Tuning (Dev)  —  Toggle: F10");
            GUILayout.Label($"Save file: {FontTuningOverrides.OverridesPath}");
            GUILayout.Space(6);

            DrawLocalePicker();
            GUILayout.Space(6);

            _scroll = GUILayout.BeginScrollView(_scroll);

            DrawFloatSlider("BestFitScaleAdjustment", ref _bestFitScale, 0.50f, 2.00f, 0.01f);
            DrawFloatSlider("HeadingScaleAdjustment", ref _headingScale, 0.50f, 2.00f, 0.01f);
            DrawFloatSlider("RegularScaleAdjustment", ref _regularScale, 0.50f, 2.00f, 0.01f);
            DrawFloatSlider("HeadingAdjustY", ref _headingY, -40f, 40f, 0.5f);
            DrawFloatSlider("RegularAdjustY", ref _regularY, -40f, 40f, 0.5f);

            GUILayout.Space(8);
            _bold = GUILayout.Toggle(_bold, "Bold (Fonts table key: Bold)");

            GUILayout.EndScrollView();

            GUILayout.Space(8);
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Apply (live)"))
            {
                PushOverrides();
                RefreshAllFontChanges();
                Toast("Applied overrides (in memory).");
            }

            if (GUILayout.Button("Save (file)"))
            {
                PushOverrides();
                FontTuningOverrides.SaveToDisk();
                Toast($"Saved: {FontTuningOverrides.OverridesPath}");
            }

            if (GUILayout.Button("Copy JSON"))
            {
                PushOverrides();
                GUIUtility.systemCopyBuffer = FontTuningOverrides.ExportJsonString();
                Toast("Copied JSON to clipboard.");
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Copy save path"))
            {
                GUIUtility.systemCopyBuffer = FontTuningOverrides.OverridesPath;
                Toast("Copied save path to clipboard.");
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Reload from tables/overrides"))
            {
                PullFromTablesOrOverrides();
                Toast("Reloaded.");
            }

            if (GUILayout.Button("Clear overrides (this locale)"))
            {
                FontTuningOverrides.ClearCurrentLocale();
                FontTuningOverrides.SaveToDisk();
                PullFromTablesOrOverrides();
                RefreshAllFontChanges();
                Toast("Cleared overrides for current locale.");
            }
            GUILayout.EndHorizontal();

            if (!string.IsNullOrEmpty(_status))
            {
                GUILayout.Space(6);
                GUILayout.Label(_status);
            }

            GUILayout.EndArea();
        }

        private void DrawLocalePicker()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Locale:", GUILayout.Width(60));

            string current = LocalizationSettings.SelectedLocale != null
                ? LocalizationSettings.SelectedLocale.LocaleName
                : "(none)";
            GUILayout.Label(current);

            if (GUILayout.Button("<", GUILayout.Width(28)))
                CycleLocale(-1);
            if (GUILayout.Button(">", GUILayout.Width(28)))
                CycleLocale(1);

            GUILayout.EndHorizontal();
        }

        private void CycleLocale(int delta)
        {
            var locales = LocalizationSettings.AvailableLocales?.Locales;
            if (locales == null || locales.Count == 0) return;

            int idx = Mathf.Max(0, locales.IndexOf(LocalizationSettings.SelectedLocale));
            int next = (idx + delta) % locales.Count;
            if (next < 0) next += locales.Count;

            LocalizationSettings.SelectedLocale = locales[next];
            PullFromTablesOrOverrides();
            RefreshAllFontChanges();
            Toast($"Switched locale: {LocalizationSettings.SelectedLocale.LocaleName}");
        }

        private void DrawFloatSlider(string label, ref float value, float min, float max, float step)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, GUILayout.Width(190));
            float newValue = GUILayout.HorizontalSlider(value, min, max);
            newValue = Mathf.Round(newValue / step) * step;
            value = newValue;
            GUILayout.Label(value.ToString("0.###", CultureInfo.InvariantCulture), GUILayout.Width(70));
            GUILayout.EndHorizontal();
        }

        private void PullFromTablesOrOverrides()
        {
            _bestFitScale = ReadFloat("BestFitScaleAdjustment", 1.0f);
            _headingScale = ReadFloat("HeadingScaleAdjustment", 1.0f);
            _regularScale = ReadFloat("RegularScaleAdjustment", 1.0f);
            _headingY = ReadFloat("HeadingAdjustY", 0f);
            _regularY = ReadFloat("RegularAdjustY", 0f);
            _bold = ReadBool("Bold", false);
        }

        private void PushOverrides()
        {
            FontTuningOverrides.SetFloatForCurrentLocale("BestFitScaleAdjustment", _bestFitScale);
            FontTuningOverrides.SetFloatForCurrentLocale("HeadingScaleAdjustment", _headingScale);
            FontTuningOverrides.SetFloatForCurrentLocale("RegularScaleAdjustment", _regularScale);
            FontTuningOverrides.SetFloatForCurrentLocale("HeadingAdjustY", _headingY);
            FontTuningOverrides.SetFloatForCurrentLocale("RegularAdjustY", _regularY);
            FontTuningOverrides.SetBoolForCurrentLocale("Bold", _bold);
        }

        private float ReadFloat(string key, float fallback)
        {
            // Override file first.
            if (FontTuningOverrides.TryGetString(key, out string s) && TryParseFloat(s, out float v))
                return v;

            // Localization table.
            string fromTable = LocTableCache.FontsTable.GetString(key);
            if (TryParseFloat(fromTable, out float tv))
                return tv;

            // Defensive trailing-space key.
            if (!key.EndsWith(" ", StringComparison.Ordinal))
            {
                string spaced = LocTableCache.FontsTable.GetString(key + " ");
                if (TryParseFloat(spaced, out float sv))
                    return sv;
            }

            return fallback;
        }

        private bool ReadBool(string key, bool fallback)
        {
            if (FontTuningOverrides.TryGetString(key, out string s) && bool.TryParse(s, out bool v))
                return v;

            string fromTable = LocTableCache.FontsTable.GetString(key);
            if (bool.TryParse(fromTable, out bool tv))
                return tv;

            return fallback;
        }

        private static bool TryParseFloat(string s, out float value)
        {
            return float.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
        }

        private static void RefreshAllFontChanges()
        {
            var all = UnityEngine.Object.FindObjectsOfType<LocalisationFontChanges>(true);
            foreach (var item in all)
                item.RefreshFontChanges();
        }

        private void Toast(string message)
        {
            _status = message;
            _statusUntil = Time.unscaledTime + 3.0f;
        }
    }
}


