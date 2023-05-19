using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers.Sprites
{
    public class PvPSpriteFetcher : IPvPSpriteFetcher
    {
        public async Task<IPvPSpriteWrapper> GetSpriteAsync(string spritePath)
        {
            AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>(spritePath);
            await handle.Task;

            if (handle.Status != AsyncOperationStatus.Succeeded
                || handle.Result == null)
            {
                throw new ArgumentException("Failed to retrieve sprite for path: " + spritePath);
            }

            return new PvPSpriteWrapper(handle.Result);
        }

        public async Task<IList<IPvPSpriteWrapper>> GetMultiSpritesAsync(string spritePath)
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
                    .Select(sprite => (IPvPSpriteWrapper)new PvPSpriteWrapper(sprite))
                    .ToList();
        }
    }
}
