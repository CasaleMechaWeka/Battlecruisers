using BattleCruisers.Movement.Velocity.Providers;
using BattleCruisers.Utils;
using System.Threading.Tasks;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft.SpriteChoosers
{
    public interface IPvPSpriteChooserFactory
    {
        Task<IPvPSpriteChooser> CreateAircraftSpriteChooserAsync(PrefabKeyName prefabKeyName, IVelocityProvider maxVelocityProvider);
        IPvPSpriteChooser CreateDummySpriteChooser(Sprite sprite);
    }
}
