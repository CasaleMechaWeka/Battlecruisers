using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Ensures Gradle templates exist in the project by copying Unity's built-ins.
/// If launcherTemplate.gradle is missing from the Unity install, a safe fallback
/// is written so builds do not fail on missing template errors.
/// </summary>
[InitializeOnLoad]
public static class GradleTemplateRecovery
{
    // Unity 2022.3.62f3 expected built-in template names
    private static readonly string[] BuiltInTemplates =
    {
        "baseProjectTemplate.gradle",
        "mainTemplate.gradle",
        "settingsTemplate.gradle",
        "libTemplate.gradle",
        "gradleTemplate.properties",
    };

    // Fallback launcher template (Unity sometimes omits this file from install)
    private const string LauncherTemplateFileName = "launcherTemplate.gradle";

    private const string LauncherFallback = @"apply plugin: 'com.android.application'

dependencies {
    implementation project(':unityLibrary')
}

android {
    namespace '**APPLICATIONID**'
    ndkPath ""**NDKPATH**""

    compileSdkVersion **APIVERSION**
    buildToolsVersion '**BUILDTOOLS**'

    defaultConfig {
        applicationId '**APPLICATIONID**'
        minSdkVersion **MINSDKVERSION**
        targetSdkVersion **TARGETSDKVERSION**
        ndk {
            abiFilters **ABIFILTERS**
        }
        versionCode **VERSIONCODE**
        versionName '**VERSIONNAME**'
        consumerProguardFiles 'proguard-unity.txt'**USER_PROGUARD**
        multiDexEnabled true
    }

    compileOptions {
        sourceCompatibility JavaVersion.VERSION_11
        targetCompatibility JavaVersion.VERSION_11
    }

    lintOptions {
        abortOnError false
    }

    buildTypes {
        debug {
            minifyEnabled **MINIFY_DEBUG**
            proguardFiles getDefaultProguardFile('proguard-android.txt')
            signingConfig signingConfigs.debug
            jniDebuggable true
        }
        release {
            minifyEnabled **MINIFY_RELEASE**
            proguardFiles getDefaultProguardFile('proguard-android.txt')
            signingConfig signingConfigs.debug
        }
    }
**PACKAGING_OPTIONS**
    bundle {
        language { enableSplit = false }
        density { enableSplit = false }
        abi { enableSplit = true }
    }
}

apply plugin: 'com.google.gms.google-services'
";

    static GradleTemplateRecovery()
    {
        // Run once on editor load (but no-op if Unity not in an editor context)
        EditorApplication.delayCall += EnsureTemplates;
    }

    [MenuItem("Tools/Battlecruisers/Recover Gradle Templates")]
    public static void EnsureTemplates()
    {
        string editorRoot = GetEditorRootPath();
        if (string.IsNullOrEmpty(editorRoot))
        {
            Debug.LogWarning("[GradleTemplateRecovery] Unable to find Unity Editor install path.");
            return;
        }

        string sourceDir = Path.Combine(editorRoot, "Data", "PlaybackEngines", "AndroidPlayer", "Tools", "GradleTemplates");
        string targetDir = Path.Combine(Application.dataPath, "Plugins", "Android");
        Directory.CreateDirectory(targetDir);

        foreach (var fileName in BuiltInTemplates)
        {
            CopyIfMissing(sourceDir, targetDir, fileName);
        }

        // Handle launcher template separately (Unity 2022.3 installer may omit it)
        string launcherTarget = Path.Combine(targetDir, LauncherTemplateFileName);
        if (!File.Exists(launcherTarget))
        {
            string launcherSource = Path.Combine(sourceDir, LauncherTemplateFileName);
            if (File.Exists(launcherSource))
            {
                File.Copy(launcherSource, launcherTarget, overwrite: false);
                Debug.Log($"[GradleTemplateRecovery] Copied launcher template from Editor: {launcherTarget}");
            }
            else
            {
                File.WriteAllText(launcherTarget, LauncherFallback);
                Debug.LogWarning("[GradleTemplateRecovery] Editor launcher template missing; wrote fallback launcherTemplate.gradle to project.");
            }
        }
    }

    private static void CopyIfMissing(string sourceDir, string targetDir, string fileName)
    {
        string source = Path.Combine(sourceDir, fileName);
        string target = Path.Combine(targetDir, fileName);

        if (File.Exists(target))
        {
            return; // Respect project copy
        }

        if (File.Exists(source))
        {
            File.Copy(source, target, overwrite: false);
            Debug.Log($"[GradleTemplateRecovery] Copied {fileName} from Editor.");
        }
        else
        {
            Debug.LogWarning($"[GradleTemplateRecovery] Missing in Editor: {source}");
        }
    }

    private static string GetEditorRootPath()
    {
        // Typical Windows Hub path; adjust if needed
        string editorExe = EditorApplication.applicationPath;
        if (string.IsNullOrEmpty(editorExe))
            return null;
        return Path.GetDirectoryName(editorExe);
    }
}

