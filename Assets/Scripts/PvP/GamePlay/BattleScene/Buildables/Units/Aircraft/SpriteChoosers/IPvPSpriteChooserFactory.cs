using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity.Providers;
using BattleCruisers.Utils;
using System.Threading.Tasks;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft.SpriteChoosers
{
    public interface IPvPSpriteChooserFactory
    {
        Task<IPvPSpriteChooser> CreateAircraftSpriteChooserAsync(PrefabKeyName prefabKeyName, IPvPVelocityProvider maxVelocityProvider);
        IPvPSpriteChooser CreateDummySpriteChooser(Sprite sprite);
    }
}
