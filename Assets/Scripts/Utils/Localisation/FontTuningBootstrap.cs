using UnityEngine;
using System.IO;

namespace BattleCruisers.Utils.Localisation
{
    /// <summary>
    /// Spawns the font tuning overlay without needing any scene edits.
    /// Default behavior is non-invasive: it only activates when explicitly enabled.
    /// </summary>
    public static class FontTuningBootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Init()
        {
            if (!IsEnabled()) return;
            if (Object.FindFirstObjectByType<FontTuningOverlay>() != null) return;
            var go = new GameObject("FontTuningOverlay (Dev)");
            go.AddComponent<FontTuningOverlay>();
            Object.DontDestroyOnLoad(go);
        }

        private static bool IsEnabled()
        {
            // Special branch flag: enable the tuner in all builds for localization team.
#if LOC_TEAM
            return true;
#endif

            // Always allow in Editor / Development builds.
            if (Debug.isDebugBuild) return true;

            // Allow opt-in in Release builds via Steam launch options:
            //   -fonttuner
            // Or via enable file in persistentDataPath:
            //   enable_font_tuner.txt
            string[] args = System.Environment.GetCommandLineArgs();
            for (int i = 0; i < args.Length; i++)
            {
                if (string.Equals(args[i], "-fonttuner", System.StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            try
            {
                string enablePath = Path.Combine(Application.persistentDataPath, "enable_font_tuner.txt");
                return File.Exists(enablePath);
            }
            catch
            {
                return false;
            }
        }
    }
}


