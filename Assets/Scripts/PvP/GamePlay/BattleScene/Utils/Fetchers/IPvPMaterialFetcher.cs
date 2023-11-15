using System.Threading.Tasks;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers
{
    public interface IPvPMaterialFetcher
    {
        Task<Material> GetMaterialAsync(string materialName);
    }
}
