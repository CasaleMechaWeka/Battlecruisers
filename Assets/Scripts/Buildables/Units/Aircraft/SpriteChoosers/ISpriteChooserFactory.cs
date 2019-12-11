using BattleCruisers.Movement.Velocity.Providers;
using System.Threading.Tasks;
using UnityEngine;

namespace BattleCruisers.Buildables.Units.Aircraft.SpriteChoosers
{
    public interface ISpriteChooserFactory
    {
        Task<ISpriteChooser> CreateBomberSpriteChooserAsync(IVelocityProvider maxVelocityProvider);
        Task<ISpriteChooser> CreateFighterSpriteChooserAsync(IVelocityProvider maxVelocityProvider);
        Task<ISpriteChooser> CreateGunshipSpriteChooserAsync(IVelocityProvider maxVelocityProvider);
        ISpriteChooser CreateDummySpriteChooser(Sprite sprite);
    }
}
