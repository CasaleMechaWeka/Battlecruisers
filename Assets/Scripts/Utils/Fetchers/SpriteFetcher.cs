using BattleCruisers.Utils.PlatformAbstractions.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace BattleCruisers.Utils.Fetchers
{
    public class SpriteFetcher : ISpriteFetcher
	{
        public async Task<IList<ISpriteWrapper>> GetMultiSpritesAsync(string spritePath)
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
                    .Select(sprite => (ISpriteWrapper)new SpriteWrapper(sprite))
                    .ToList();
		}
	}
}
