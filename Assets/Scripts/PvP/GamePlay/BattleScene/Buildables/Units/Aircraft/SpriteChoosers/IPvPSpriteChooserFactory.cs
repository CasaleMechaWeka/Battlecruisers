using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity.Providers;
using System.Threading.Tasks;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft.SpriteChoosers
{
    public interface IPvPSpriteChooserFactory
    {
        Task<IPvPSpriteChooser> CreateBomberSpriteChooserAsync(IPvPVelocityProvider maxVelocityProvider);
        Task<IPvPSpriteChooser> CreateFighterSpriteChooserAsync(IPvPVelocityProvider maxVelocityProvider);
        Task<IPvPSpriteChooser> CreateGunshipSpriteChooserAsync(IPvPVelocityProvider maxVelocityProvider);
        Task<IPvPSpriteChooser> CreateSteamCopterSpriteChooserAsync(IPvPVelocityProvider maxVelocityProvider);

        IPvPSpriteChooser CreateDummySpriteChooser(Sprite sprite);
    }
}
