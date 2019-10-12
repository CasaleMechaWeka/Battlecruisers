using System.Threading.Tasks;
using UnityEngine;

namespace BattleCruisers.UI.Sound
{
    public interface ISoundPlayer
    {
        Task PlaySoundAsync(ISoundKey soundKey);
        Task PlaySoundAsync(ISoundKey soundKey, Vector2 position);
    }
}
