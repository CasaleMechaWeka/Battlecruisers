using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Conditionally excludes AppLovin MAX dependencies when DISABLE_ADS is defined.
/// This allows building without AppLovin SDK when ads are disabled.
/// </summary>
[InitializeOnLoad]
public static class AppLovinDependencyConditional
{
    private const string APPLOVIN_DEPENDENCIES_PATH = "Assets/Editor/AppLovinMaxDependencies.xml";
    private const string APPLOVIN_DEPENDENCIES_BACKUP = "Assets/Editor/AppLovinMaxDependencies.xml.backup";
    private const string DISABLE_ADS_DEFINE = "DISABLE_ADS";

    static AppLovinDependencyConditional()
    {
        // Check if DISABLE_ADS is defined
        bool disableAds = IsDefineEnabled(DISABLE_ADS_DEFINE);
        
        // Handle dependency file based on define
        if (disableAds)
        {
            DisableAppLovinDependencies();
        }
        else
        {
            EnableAppLovinDependencies();
        }
    }

    private static bool IsDefineEnabled(string define)
    {
        BuildTargetGroup buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
        string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
        return defines.Contains(define);
    }

    private static void DisableAppLovinDependencies()
    {
        if (!File.Exists(APPLOVIN_DEPENDENCIES_PATH))
        {
            return; // Already disabled or doesn't exist
        }

        // Backup original file
        if (!File.Exists(APPLOVIN_DEPENDENCIES_BACKUP))
        {
            File.Copy(APPLOVIN_DEPENDENCIES_PATH, APPLOVIN_DEPENDENCIES_BACKUP, true);
            Debug.Log("[AppLovin] Backed up AppLovinMaxDependencies.xml (DISABLE_ADS is enabled)");
        }

        // Create empty/disabled version
        string disabledContent = @"<!-- AppLovin MAX SDK Dependencies - DISABLED (DISABLE_ADS is defined) -->
<!-- This file is automatically disabled when DISABLE_ADS scripting define is set -->
<dependencies>
  <!-- All AppLovin dependencies are excluded when DISABLE_ADS is enabled -->
</dependencies>
";
        File.WriteAllText(APPLOVIN_DEPENDENCIES_PATH, disabledContent);
        Debug.Log("[AppLovin] AppLovin dependencies disabled (DISABLE_ADS is enabled)");
        
        // Force Android Resolver to re-resolve
        AssetDatabase.Refresh();
    }

    private static void EnableAppLovinDependencies()
    {
        if (!File.Exists(APPLOVIN_DEPENDENCIES_BACKUP))
        {
            return; // No backup exists, file is already in original state
        }

        // Restore original file
        File.Copy(APPLOVIN_DEPENDENCIES_BACKUP, APPLOVIN_DEPENDENCIES_PATH, true);
        Debug.Log("[AppLovin] AppLovin dependencies enabled (DISABLE_ADS is not defined)");
        
        // Force Android Resolver to re-resolve
        AssetDatabase.Refresh();
    }

    [MenuItem("Tools/AppLovin/Toggle DISABLE_ADS Define")]
    private static void ToggleDisableAds()
    {
        BuildTargetGroup buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
        string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
        
        bool isEnabled = defines.Contains(DISABLE_ADS_DEFINE);
        
        if (isEnabled)
        {
            // Remove define
            defines = defines.Replace($"{DISABLE_ADS_DEFINE};", "").Replace($";{DISABLE_ADS_DEFINE}", "").Replace(DISABLE_ADS_DEFINE, "");
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, defines);
            Debug.Log("[AppLovin] DISABLE_ADS define removed - AppLovin will be included in builds");
        }
        else
        {
            // Add define
            if (!string.IsNullOrEmpty(defines) && !defines.EndsWith(";"))
            {
                defines += ";";
            }
            defines += DISABLE_ADS_DEFINE;
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, defines);
            Debug.Log("[AppLovin] DISABLE_ADS define added - AppLovin will be excluded from builds");
        }
        
        // Force recompilation
        AssetDatabase.Refresh();
    }
}

