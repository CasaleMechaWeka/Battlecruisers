using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.Build.Reporting;
using UnityEngine;

/// <summary>
/// Clean build script that clears all caches, rebuilds addressables, 
/// force resolves Android dependencies, and builds the APK.
/// 
/// IMPORTANT: This project uses AppLovin MAX ONLY (not LevelPlay/IronSource).
/// See PROJECT_DOCUMENTATION.md for full details.
/// </summary>
public static class CleanBuildAndroid
{
    private static readonly string[] CacheFoldersToDelete = new[]
    {
        "Library/Bee",
        "Library/BuildPlayerData",
        "Library/Il2cppBuildCache",
        "Library/ScriptAssemblies",
        "Library/com.unity.addressables",
        "Library/PackageCache",
        "Temp"
    };

    private static readonly string[] GradleCachePaths = new[]
    {
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".gradle/caches"),
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".gradle/wrapper"),
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Unity/cache/packages/packages.unity.com")
    };

    // Stale dependencies from old LevelPlay/IronSource that should be removed
    private static readonly string[] StaleDependencyPatterns = new[]
    {
        "com.ironsource",
        "com.unity3d.mediation",
        "com.unity3d.ads-mediation",
        "unity-levelplay",
        "mediationsdk",
        "unityadsadapter"
    };

    [MenuItem("Build/Clean Build Android (Full Reset)", priority = 100)]
    public static void CleanBuildFull()
    {
        if (!EditorUtility.DisplayDialog("Clean Build Android",
            "This will:\n" +
            "1. Delete Unity build caches\n" +
            "2. Delete Gradle caches\n" +
            "3. Clear Android Resolver cache\n" +
            "4. Rebuild Addressables\n" +
            "5. Force Resolve Android dependencies\n" +
            "6. Build Android APK\n\n" +
            "This may take a long time. Continue?",
            "Yes, Clean Build", "Cancel"))
        {
            return;
        }

        try
        {
            EditorUtility.DisplayProgressBar("Clean Build", "Step 1/6: Deleting Unity caches...", 0.1f);
            DeleteUnityCaches();

            EditorUtility.DisplayProgressBar("Clean Build", "Step 2/6: Deleting Gradle caches...", 0.2f);
            DeleteGradleCaches();

            EditorUtility.DisplayProgressBar("Clean Build", "Step 3/6: Clearing Android Resolver cache...", 0.3f);
            ClearAndroidResolverCache();

            EditorUtility.DisplayProgressBar("Clean Build", "Step 4/6: Building Addressables...", 0.5f);
            BuildAddressables();

            EditorUtility.DisplayProgressBar("Clean Build", "Step 5/6: Force Resolving Android dependencies...", 0.7f);
            ForceResolveAndroidDependencies();

            EditorUtility.DisplayProgressBar("Clean Build", "Step 6/6: Building Android APK...", 0.9f);
            BuildAndroid();

            EditorUtility.ClearProgressBar();
            Debug.Log("<color=green>[CleanBuild] Build completed successfully!</color>");
        }
        catch (Exception ex)
        {
            EditorUtility.ClearProgressBar();
            Debug.LogError($"[CleanBuild] Build failed: {ex.Message}\n{ex.StackTrace}");
            EditorUtility.DisplayDialog("Build Failed", ex.Message, "OK");
        }
    }

    [MenuItem("Build/Step 1 - Delete All Caches", priority = 200)]
    public static void DeleteAllCaches()
    {
        if (!EditorUtility.DisplayDialog("Delete All Caches",
            "This will delete Unity and Gradle caches. Continue?",
            "Yes", "Cancel"))
        {
            return;
        }

        DeleteUnityCaches();
        DeleteGradleCaches();
        ClearAndroidResolverCache();
        CleanStaleDependencies();
        Debug.Log("<color=green>[CleanBuild] All caches deleted!</color>");
    }

    [MenuItem("Build/Clean Stale LevelPlay Dependencies", priority = 150)]
    public static void CleanStaleDependenciesMenu()
    {
        CleanStaleDependencies();
        Debug.Log("<color=green>[CleanBuild] Stale dependencies cleaned! Run Force Resolve to regenerate.</color>");
        EditorUtility.DisplayDialog("Clean Complete", 
            "Stale LevelPlay/IronSource dependencies removed.\n\n" +
            "Now run: Assets > External Dependency Manager > Android Resolver > Force Resolve", 
            "OK");
    }

    [MenuItem("Build/Step 2 - Build Addressables Only", priority = 201)]
    public static void BuildAddressablesOnly()
    {
        BuildAddressables();
    }

    [MenuItem("Build/Step 3 - Force Resolve Android", priority = 202)]
    public static void ForceResolveOnly()
    {
        ForceResolveAndroidDependencies();
    }

    [MenuItem("Build/Step 4 - Build Android APK", priority = 203)]
    public static void BuildAndroidOnly()
    {
        BuildAndroid();
    }

    private static void DeleteUnityCaches()
    {
        string projectPath = Path.GetDirectoryName(Application.dataPath);
        int deletedCount = 0;

        foreach (string relativePath in CacheFoldersToDelete)
        {
            string fullPath = Path.Combine(projectPath, relativePath);
            if (Directory.Exists(fullPath))
            {
                try
                {
                    Directory.Delete(fullPath, true);
                    Debug.Log($"[CleanBuild] Deleted: {relativePath}");
                    deletedCount++;
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"[CleanBuild] Could not delete {relativePath}: {ex.Message}");
                }
            }
        }

        // Also delete generated Gradle project
        string gradleProjectPath = Path.Combine(projectPath, "Library/Bee/Android/Prj");
        if (Directory.Exists(gradleProjectPath))
        {
            try
            {
                Directory.Delete(gradleProjectPath, true);
                Debug.Log("[CleanBuild] Deleted: Library/Bee/Android/Prj (generated Gradle project)");
                deletedCount++;
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[CleanBuild] Could not delete Gradle project: {ex.Message}");
            }
        }

        Debug.Log($"[CleanBuild] Deleted {deletedCount} Unity cache folders");
    }

    private static void DeleteGradleCaches()
    {
        int deletedCount = 0;

        foreach (string cachePath in GradleCachePaths)
        {
            if (Directory.Exists(cachePath))
            {
                try
                {
                    Directory.Delete(cachePath, true);
                    Debug.Log($"[CleanBuild] Deleted Gradle cache: {cachePath}");
                    deletedCount++;
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"[CleanBuild] Could not delete Gradle cache {cachePath}: {ex.Message}");
                }
            }
        }

        Debug.Log($"[CleanBuild] Deleted {deletedCount} Gradle cache folders");
    }

    private static void ClearAndroidResolverCache()
    {
        // Try to invoke the Android Resolver's delete resolved libraries method
        try
        {
            // Find the PlayServicesResolver type
            Type resolverType = FindType("GooglePlayServices.PlayServicesResolver") 
                             ?? FindType("Google.JarResolver.PlayServicesResolver");

            if (resolverType != null)
            {
                // Call DeleteResolvedLibraries
                MethodInfo deleteMethod = resolverType.GetMethod("DeleteResolvedLibraries", 
                    BindingFlags.Public | BindingFlags.Static);
                
                if (deleteMethod != null)
                {
                    deleteMethod.Invoke(null, null);
                    Debug.Log("[CleanBuild] Cleared Android Resolver libraries");
                }
                else
                {
                    Debug.LogWarning("[CleanBuild] Could not find DeleteResolvedLibraries method");
                }
            }
            else
            {
                Debug.LogWarning("[CleanBuild] Could not find PlayServicesResolver type - manually delete Plugins/Android/*.aar if needed");
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"[CleanBuild] Could not clear Android Resolver cache: {ex.Message}");
        }

        // Also delete the resolved libraries folder
        string resolvedLibsPath = Path.Combine(Application.dataPath, "Plugins/Android/resolved-libs");
        if (Directory.Exists(resolvedLibsPath))
        {
            try
            {
                Directory.Delete(resolvedLibsPath, true);
                Debug.Log("[CleanBuild] Deleted Plugins/Android/resolved-libs");
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[CleanBuild] Could not delete resolved-libs: {ex.Message}");
            }
        }
    }

    private static void CleanStaleDependencies()
    {
        Debug.Log("[CleanBuild] Cleaning stale LevelPlay/IronSource dependencies...");
        
        // Delete stale LevelPlay dependency XML files if they still exist
        string[] staleLevelPlayFiles = new[]
        {
            "Assets/LevelPlay/Editor/LevelPlayDependencies.xml",
            "Assets/LevelPlay/Editor/IronSourceSDKDependencies.xml",
            "Assets/LevelPlay/Editor/ISAppLovinAdapterDependencies.xml",
            "Assets/LevelPlay/Editor/ISUnityAdsAdapterDependencies.xml"
        };
        
        string projectPath = Path.GetDirectoryName(Application.dataPath);
        int deletedCount = 0;
        
        foreach (string relativePath in staleLevelPlayFiles)
        {
            string fullPath = Path.Combine(projectPath, relativePath);
            if (File.Exists(fullPath))
            {
                try
                {
                    File.Delete(fullPath);
                    Debug.Log($"[CleanBuild] Deleted stale dependency file: {relativePath}");
                    deletedCount++;
                    
                    // Also delete .meta file
                    string metaPath = fullPath + ".meta";
                    if (File.Exists(metaPath))
                    {
                        File.Delete(metaPath);
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"[CleanBuild] Could not delete {relativePath}: {ex.Message}");
                }
            }
        }
        
        // Clean mainTemplate.gradle of stale dependencies
        string mainTemplatePath = Path.Combine(Application.dataPath, "Plugins/Android/mainTemplate.gradle");
        if (File.Exists(mainTemplatePath))
        {
            try
            {
                string content = File.ReadAllText(mainTemplatePath);
                string originalContent = content;
                
                // Remove lines containing stale dependency patterns
                string[] lines = content.Split('\n');
                var cleanedLines = lines.Where(line => 
                {
                    foreach (string pattern in StaleDependencyPatterns)
                    {
                        if (line.Contains(pattern))
                        {
                            Debug.Log($"[CleanBuild] Removing stale dependency: {line.Trim()}");
                            return false;
                        }
                    }
                    return true;
                }).ToList();
                
                string cleanedContent = string.Join("\n", cleanedLines);
                
                if (cleanedContent != originalContent)
                {
                    File.WriteAllText(mainTemplatePath, cleanedContent);
                    Debug.Log("[CleanBuild] Cleaned stale dependencies from mainTemplate.gradle");
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[CleanBuild] Could not clean mainTemplate.gradle: {ex.Message}");
            }
        }
        
        Debug.Log($"[CleanBuild] Cleaned {deletedCount} stale dependency files");
    }

    private static void BuildAddressables()
    {
        Debug.Log("[CleanBuild] Building Addressables...");

        AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
        if (settings == null)
        {
            Debug.LogError("[CleanBuild] Addressables settings not found!");
            return;
        }

        // Clean the Addressables build cache
        AddressableAssetSettings.CleanPlayerContent(settings.ActivePlayerDataBuilder);
        Debug.Log("[CleanBuild] Cleaned Addressables player content");

        // Build Addressables
        AddressableAssetSettings.BuildPlayerContent(out AddressablesPlayerBuildResult result);

        if (!string.IsNullOrEmpty(result.Error))
        {
            throw new Exception($"Addressables build failed: {result.Error}");
        }

        Debug.Log($"<color=green>[CleanBuild] Addressables built successfully! Duration: {result.Duration:F2}s</color>");
    }

    private static void ForceResolveAndroidDependencies()
    {
        Debug.Log("[CleanBuild] Force resolving Android dependencies...");

        try
        {
            // Find the PlayServicesResolver type using reflection
            Type resolverType = FindType("GooglePlayServices.PlayServicesResolver")
                             ?? FindType("Google.JarResolver.PlayServicesResolver");

            if (resolverType != null)
            {
                // Call Resolve with forceResolution = true
                MethodInfo resolveMethod = resolverType.GetMethod("Resolve",
                    BindingFlags.Public | BindingFlags.Static,
                    null,
                    new Type[] { typeof(bool), typeof(bool) },
                    null);

                if (resolveMethod != null)
                {
                    // Parameters: (forceResolution, closeAutomatically)
                    resolveMethod.Invoke(null, new object[] { true, true });
                    Debug.Log("<color=green>[CleanBuild] Android dependencies force resolved!</color>");
                }
                else
                {
                    // Try alternative method signature
                    MethodInfo altResolveMethod = resolverType.GetMethod("ResolveSync",
                        BindingFlags.Public | BindingFlags.Static);

                    if (altResolveMethod != null)
                    {
                        altResolveMethod.Invoke(null, new object[] { true });
                        Debug.Log("<color=green>[CleanBuild] Android dependencies force resolved (sync)!</color>");
                    }
                    else
                    {
                        Debug.LogWarning("[CleanBuild] Could not find Resolve method. " +
                            "Please manually run: Assets > External Dependency Manager > Android Resolver > Force Resolve");
                    }
                }
            }
            else
            {
                Debug.LogError("[CleanBuild] External Dependency Manager not found! " +
                    "Please install it from: https://github.com/googlesamples/unity-jar-resolver");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"[CleanBuild] Force resolve failed: {ex.Message}\n" +
                "Please manually run: Assets > External Dependency Manager > Android Resolver > Force Resolve");
        }
    }

    private static void BuildAndroid()
    {
        Debug.Log("[CleanBuild] Building Android APK...");

        // Get scenes to build
        string[] scenes = EditorBuildSettings.scenes
            .Where(s => s.enabled)
            .Select(s => s.path)
            .ToArray();

        if (scenes.Length == 0)
        {
            throw new Exception("No scenes found in build settings!");
        }

        // Build path
        string buildPath = Path.Combine(Path.GetDirectoryName(Application.dataPath), "Builds/Android");
        Directory.CreateDirectory(buildPath);

        string apkPath = Path.Combine(buildPath, $"{PlayerSettings.productName}.apk");

        BuildPlayerOptions buildOptions = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = apkPath,
            target = BuildTarget.Android,
            options = BuildOptions.None
        };

        BuildReport report = BuildPipeline.BuildPlayer(buildOptions);

        if (report.summary.result != BuildResult.Succeeded)
        {
            throw new Exception($"Build failed with {report.summary.totalErrors} errors");
        }

        Debug.Log($"<color=green>[CleanBuild] APK built successfully: {apkPath}</color>");
        Debug.Log($"[CleanBuild] Build time: {report.summary.totalTime}");
    }

    private static Type FindType(string typeName)
    {
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            try
            {
                Type type = assembly.GetType(typeName);
                if (type != null) return type;
            }
            catch
            {
                // Ignore assembly load errors
            }
        }
        return null;
    }
}
