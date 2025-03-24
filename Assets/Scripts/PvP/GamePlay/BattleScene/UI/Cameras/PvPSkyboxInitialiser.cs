using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Utils.Fetchers;
using System.Threading.Tasks;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras
{
    public class PvPSkyboxInitialiser : MonoBehaviour
    {
        public async Task InitialiseAsync(Skybox skybox, IPvPLevel level)
        {
            PvPHelper.AssertIsNotNull(skybox, level);

            skybox.material = await MaterialFetcher.GetMaterialAsync(level.SkyMaterialName);
        }
    }
}