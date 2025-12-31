using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace BattleCruisers.Utils.Fetchers.Sprites
{

    public static class SpritePaths
    {
        public const string ExoImagesPath = "Assets/Resources_moved/Sprites/UI/ScreensScene/LevelsScreen/NPCs/";
        public const string RankImagesPath = "Assets/Resources_moved/Sprites/UI/ScreensScene/DestructionScore/";
        public const string BackgroundImagesPath = "Assets/Resources_moved/Sprites/Skies/Backgrounds/";
    }

    public static class SpriteFetcher
    {
        public static async Task<Sprite> GetSpriteAsync(string spritePath)
        {
            AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>(spritePath);
            await handle.Task;

            if (handle.Status != AsyncOperationStatus.Succeeded
                || handle.Result == null)
            {
                throw new ArgumentException("Failed to retrieve sprite for path: " + spritePath);
            }

            return handle.Result;
        }

        public static async Task<IList<Sprite>> GetMultiSpritesAsync(string spritePath)
        {
            AsyncOperationHandle<IList<Sprite>> handle = Addressables.LoadAssetAsync<IList<Sprite>>(spritePath);
            await handle.Task;

            if (handle.Status != AsyncOperationStatus.Succeeded
                || handle.Result == null)
            {
                throw new ArgumentException("Failed to retrieve sprites for path: " + spritePath);
            }

            return
                handle.Result
                    .Select(sprite => sprite)
                    .ToList();
        }
    }
}
