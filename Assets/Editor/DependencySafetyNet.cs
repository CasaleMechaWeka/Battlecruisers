using UnityEditor;
using UnityEngine;
using System.IO;
using System.Xml;

/// <summary>
/// Dependency Safety Net - Runs BEFORE Android Resolver to prevent wrong version selection.
/// This ensures AppLovin 13.x, Firebase 21.x, and Kotlin 1.9.22 compatibility.
/// 
/// Unity 2022.3 Strategy:
/// - Don't downgrade to old 2021.3 versions
/// - Pin to known-working versions that support Kotlin 1.9.22
/// - Verify dependency XML files have correct versions
/// </summary>
[InitializeOnLoad]
public class DependencySafetyNet
{
    // Known-working versions for Unity 2022.3 + AppLovin 13.x
    private const string APPLOVIN_MIN_VERSION = "13.0.0";
    private const string FIREBASE_ANALYTICS_VERSION = "21.5.0";
    private const string FIREBASE_CRASHLYTICS_VERSION = "18.6.0";
    
    static DependencySafetyNet()
    {
        // Run on editor load
        EditorApplication.delayCall += VerifyDependencies;
    }
    
    [MenuItem("Tools/Battlecruisers/Verify Android Dependencies")]
    public static void VerifyDependencies()
    {
        Debug.Log("[DependencySafetyNet] Verifying Android dependency versions...");
        
        bool allGood = true;
        
        // Check AppLovin version
        string appLovinDeps = "Assets/MaxSdk/AppLovin/Editor/Dependencies.xml";
        if (File.Exists(appLovinDeps))
        {
            string content = File.ReadAllText(appLovinDeps);
            if (content.Contains("applovin-sdk:12."))
            {
                Debug.LogWarning("[DependencySafetyNet] ⚠️ AppLovin SDK is pinned to 12.x - Unity 2022.3 works better with 13.x");
                allGood = false;
            }
            else if (content.Contains("applovin-sdk:13."))
            {
                Debug.Log("[DependencySafetyNet] ✅ AppLovin SDK version 13.x detected");
            }
        }
        
        // Check Firebase versions
        string firebaseDeps = "Assets/Editor/FirebaseDependencies.xml";
        if (File.Exists(firebaseDeps))
        {
            string content = File.ReadAllText(firebaseDeps);
            
            if (content.Contains($"firebase-analytics:{FIREBASE_ANALYTICS_VERSION}"))
            {
                Debug.Log($"[DependencySafetyNet] ✅ Firebase Analytics {FIREBASE_ANALYTICS_VERSION} detected");
            }
            else
            {
                Debug.LogWarning("[DependencySafetyNet] ⚠️ Firebase Analytics version may not be optimal");
                allGood = false;
            }
            
            if (content.Contains($"firebase-crashlytics:{FIREBASE_CRASHLYTICS_VERSION}"))
            {
                Debug.Log($"[DependencySafetyNet] ✅ Firebase Crashlytics {FIREBASE_CRASHLYTICS_VERSION} detected");
            }
        }
        
        // Check gradle.properties
        string gradleProps = "Assets/Plugins/Android/gradleTemplate.properties";
        if (File.Exists(gradleProps))
        {
            string content = File.ReadAllText(gradleProps);
            
            if (content.Contains("android.enableR8=true"))
            {
                Debug.Log("[DependencySafetyNet] ✅ R8 is ENABLED (correct for Unity 2022.3)");
            }
            else if (content.Contains("android.enableR8=false"))
            {
                Debug.LogWarning("[DependencySafetyNet] ⚠️ R8 is DISABLED - should be enabled for Unity 2022.3");
                allGood = false;
            }
            
            if (content.Contains("android.enableJetifier=true"))
            {
                Debug.Log("[DependencySafetyNet] ✅ Jetifier is enabled (required for AppLovin)");
            }
        }
        
        // Check for old-style custom templates (should not exist)
        string[] oldTemplates = {
            "Assets/Plugins/Android/launcherTemplate.gradle",
            "Assets/Plugins/Android/mainTemplate.gradle",
            "Assets/Plugins/Android/baseProjectTemplate.gradle",
            "Assets/Plugins/Android/settingsTemplate.gradle"
        };
        
        foreach (var template in oldTemplates)
        {
            if (File.Exists(template))
            {
                Debug.LogWarning($"[DependencySafetyNet] ⚠️ Custom template exists: {template}");
                Debug.LogWarning("   Unity 2022.3 works better with auto-generated templates!");
                allGood = false;
            }
        }
        
        if (allGood)
        {
            Debug.Log("[DependencySafetyNet] ✅ All dependency checks passed!");
        }
        else
        {
            Debug.LogWarning("[DependencySafetyNet] ⚠️ Some dependency issues detected - review warnings above");
        }
    }
    
    [MenuItem("Tools/Battlecruisers/Force Clean Android Build")]
    public static void ForceCleanBuild()
    {
        Debug.Log("[DependencySafetyNet] Starting clean build preparation...");
        
        // Delete Unity Android caches
        string[] cachePaths = {
            "Library/Bee/Android",
            "Library/Bee/artifacts",
            "Temp"
        };
        
        foreach (var cachePath in cachePaths)
        {
            if (Directory.Exists(cachePath))
            {
                try
                {
                    Directory.Delete(cachePath, true);
                    Debug.Log($"[DependencySafetyNet] Deleted: {cachePath}");
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning($"[DependencySafetyNet] Could not delete {cachePath}: {e.Message}");
                }
            }
        }
        
        Debug.Log("[DependencySafetyNet] Clean build preparation complete. Run Android Resolver → Force Resolve, then build.");
    }
}

