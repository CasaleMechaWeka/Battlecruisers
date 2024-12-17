using BattleCruisers.Movement.Velocity.Providers;
using BattleCruisers.Utils;
using System.Threading.Tasks;
using UnityEngine;

namespace BattleCruisers.Buildables.Units.Aircraft.SpriteChoosers
{
    public interface ISpriteChooserFactory
    {
        Task<ISpriteChooser> CreateAircraftSpriteChooserAsync(PrefabKeyName prefabKeyName, IVelocityProvider maxVelocityProvider);
        ISpriteChooser CreateDummySpriteChooser(Sprite sprite);
    }
}
