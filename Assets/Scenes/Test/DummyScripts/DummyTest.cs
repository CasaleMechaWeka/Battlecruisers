using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class DummyTest : MonoBehaviour
{
    private const string SOUND_ROOT_DIR = "Assets/Resources_moved/Sounds";
    private const char PATH_SEPARATOR = '/';
    // Start is called before the first frame update
    private async void Start()
    {
        string soundPath = CreateSoundPath("anti-air");

        AsyncOperationHandle<AudioClip> handle = Addressables.LoadAssetAsync<AudioClip>(soundPath);
        await handle.Task;

        if (handle.Status != AsyncOperationStatus.Succeeded
            || handle.Result == null)
        {
            throw new ArgumentException("Failed to retrieve sound with key: " + soundPath);
        }     
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private string CreateSoundPath(string soundKey)
    {
        return "xxx";
    }
}
